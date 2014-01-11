using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.StorageClient.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using XOracle.Azure.Core.Helpers;
using XOracle.Infrastructure.Core;

namespace XOracle.Azure.Core.Stores.Storage
{
    public abstract class AzureBlobContainer<T> : AzureStorageWithRetryPolicy, IAzureBlobContainer<T>
    {
        protected const int BlobRequestTimeout = 120;

        protected readonly CloudBlobContainer Container;
        protected readonly CloudStorageAccount Account;

        private readonly IDictionary<Type, Action<IConcurrencyControlContext, T>> _writingStrategies;
        private readonly ILogger _logger;

        public AzureBlobContainer(CloudStorageAccount account)
            : this(account, typeof(T).Name.ToLowerInvariant()) { }

        public AzureBlobContainer(CloudStorageAccount account, string containerName)
        {
            this._logger = Factory<ILogger>.GetInstance();

            this.Account = account;

            var client = account.CreateCloudBlobClient();

            // retry policy is handled by TFHAB
            client.RetryPolicy = RetryPolicies.NoRetry();

            this.Container = client.GetContainerReference(containerName);

            this._writingStrategies = new Dictionary<Type, Action<IConcurrencyControlContext, T>>()
            {
                { typeof(OptimisticConcurrencyContext), this.OptimisticControlContextWriteStrategy },
                { typeof(PessimisticConcurrencyContext), this.PessimisticControlContextWriteStrategy }
            };
        }

        public void RegisterConcurrencyWriteStrategy<C>(Action<IConcurrencyControlContext, T> writeAction) where C : IConcurrencyControlContext
        {
            this._writingStrategies[typeof(C)] = writeAction;
        }

        public bool AcquireLock(PessimisticConcurrencyContext lockContext)
        {
            var request = BlobRequest.Lease(this.GetUri(lockContext.ObjectId), BlobRequestTimeout, LeaseAction.Acquire, null);
            this.Account.Credentials.SignRequest(request);

            // add extra headers not supported by SDK - not supported by emulator yet (SDK 1.7)
            ////request.Headers["x-ms-version"] = "2012-02-12";
            ////request.Headers.Add("x-ms-lease-duration", lockContext.Duration.TotalSeconds.ToString());

            try
            {
                using (var response = request.GetResponse())
                {
                    if (response is HttpWebResponse &&
                       HttpStatusCode.Created.Equals((response as HttpWebResponse).StatusCode))
                    {
                        lockContext.LockId = response.Headers["x-ms-lease-id"];
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (WebException e)
            {
                this._logger.LogWarning("Warning acquiring blob '{0}' lease: {1}", lockContext.ObjectId, e.Message);
                if (WebExceptionStatus.ProtocolError.Equals(e.Status))
                {
                    if (e.Response is HttpWebResponse)
                    {
                        if (HttpStatusCode.NotFound.Equals((e.Response as HttpWebResponse).StatusCode))
                        {
                            lockContext.LockId = null;
                            return true;
                        }
                        else if (HttpStatusCode.Conflict.Equals((e.Response as HttpWebResponse).StatusCode))
                        {
                            lockContext.LockId = null;
                            return false;
                        }
                    }
                    throw;
                }
                return false;
            }
            catch (Exception e)
            {
                this._logger.LogError("Error acquiring blob '{0}' lease: {1}", lockContext.ObjectId, e.Message);
                throw;
            }
        }

        public void ReleaseLock(PessimisticConcurrencyContext lockContext)
        {
            if (string.IsNullOrWhiteSpace(lockContext.LockId))
            {
                throw new ArgumentNullException("lockContext.LockId", "LockId cannot be null or empty");
            }

            var request = BlobRequest.Lease(this.GetUri(lockContext.ObjectId), BlobRequestTimeout, LeaseAction.Release, lockContext.LockId);
            this.Account.Credentials.SignRequest(request);

            using (var response = request.GetResponse())
            {
                if (response is HttpWebResponse &&
                    !HttpStatusCode.OK.Equals((response as HttpWebResponse).StatusCode))
                {
                    this._logger.LogError("Error releasing blob '{0}' lease: {1}", lockContext.ObjectId, (response as HttpWebResponse).StatusDescription);
                    throw new InvalidOperationException((response as HttpWebResponse).StatusDescription);
                }
            }
        }

        public virtual void Delete(string objId)
        {
            this.StorageRetryPolicy.ExecuteAction(() =>
            {
                CloudBlob blob = this.Container.GetBlobReference(objId);
                blob.DeleteIfExists();
            });
        }

        public virtual void DeleteContainer()
        {
            this.StorageRetryPolicy.ExecuteAction(() =>
            {
                try
                {
                    this.Container.Delete();
                }
                catch (StorageClientException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        this._logger.LogWarning(ex.TraceInformation());
                        return;
                    }

                    this._logger.LogError(ex.TraceInformation());

                    throw;
                }
            });
        }

        public virtual void EnsureExist()
        {
            this.StorageRetryPolicy.ExecuteAction(() => this.Container.CreateIfNotExist());
        }

        public virtual T Get(string objId)
        {
            OptimisticConcurrencyContext optimisticContext;
            return this.Get(objId, out optimisticContext);
        }

        public virtual T Get(string objId, out OptimisticConcurrencyContext context)
        {
            OptimisticConcurrencyContext optimisticContext = null;
            var result = this.StorageRetryPolicy.ExecuteAction<T>(() =>
            {
                try
                {
                    CloudBlob blob = this.Container.GetBlobReference(objId);
                    blob.FetchAttributes();
                    optimisticContext = new OptimisticConcurrencyContext(blob.Properties.ETag) { ObjectId = objId };
                    return this.ReadObject(blob);
                }
                catch (StorageClientException ex)
                {
                    this._logger.LogWarning(ex.TraceInformation());
                    if (HttpStatusCode.NotFound.Equals(ex.StatusCode) &&
                        (StorageErrorCode.BlobNotFound.Equals(ex.ErrorCode) ||
                        StorageErrorCode.ResourceNotFound.Equals(ex.ErrorCode)))
                    {
                        optimisticContext = this.GetContextForUnexistentBlob(objId);
                        return default(T);
                    }
                    throw;
                }
            });
            context = optimisticContext;
            return result;
        }

        public virtual IEnumerable<IListBlobItemWithName> GetBlobList()
        {
            return this.StorageRetryPolicy.ExecuteAction<IEnumerable<IListBlobItemWithName>>(() => this.Container.ListBlobs().Select(b => new AzureBlob(b as CloudBlob)));
        }

        public virtual Uri GetUri(string objId)
        {
            CloudBlob blob = this.Container.GetBlobReference(objId);
            return blob.Uri;
        }

        public virtual void Save(string objId, T obj)
        {
            var context = new OptimisticConcurrencyContext() { ObjectId = objId };
            this.Save(context, obj);
        }

        public virtual void Save(IConcurrencyControlContext context, T obj)
        {
            if (string.IsNullOrWhiteSpace(context.ObjectId))
            {
                throw new ArgumentNullException("context.ObjectId", "ObjectId cannot be null or empty");
            }

            Action<IConcurrencyControlContext, T> writeStrategy;
            if (!this._writingStrategies.TryGetValue(context.GetType(), out writeStrategy))
            {
                throw new InvalidOperationException("IConcurrencyControlContext implementation not registered");
            }

            this.StorageRetryPolicy.ExecuteAction(() => writeStrategy(context, obj));
        }

        protected virtual OptimisticConcurrencyContext GetContextForUnexistentBlob(string objId)
        {
            return new OptimisticConcurrencyContext()
            {
                ObjectId = objId,
                AccessCondition = AccessCondition.IfNotModifiedSince(DateTime.MinValue)
            };
        }

        protected virtual void OptimisticControlContextWriteStrategy(IConcurrencyControlContext context, T obj)
        {
            CloudBlob blob = this.Container.GetBlobReference(context.ObjectId);

            var blobRequestOptions = new BlobRequestOptions()
            {
                AccessCondition = (context as OptimisticConcurrencyContext).AccessCondition
            };

            this.WriteObject(blob, blobRequestOptions, obj);
        }

        protected virtual void PessimisticControlContextWriteStrategy(IConcurrencyControlContext context, T obj)
        {
            if (string.IsNullOrWhiteSpace((context as PessimisticConcurrencyContext).LockId))
                throw new ArgumentNullException("context.LockId", "LockId cannot be null or empty");

            var blobProperties = new BlobProperties();

            var binarizedObject = this.BinarizeObjectForStreaming(blobProperties, obj);

            var updateRequest = BlobRequest.Put(
                this.GetUri(context.ObjectId),
                BlobRequestTimeout,
                blobProperties,
                BlobType.BlockBlob,
                (context as PessimisticConcurrencyContext).LockId,
                0);

            using (var writer = new BinaryWriter(updateRequest.GetRequestStream(), Encoding.Default))
            {
                writer.Write(binarizedObject);
            }

            this.Account.Credentials.SignRequest(updateRequest);

            using (var response = updateRequest.GetResponse())
            {
                if (response is HttpWebResponse &&
                    !HttpStatusCode.Created.Equals((response as HttpWebResponse).StatusCode))
                {
                    this._logger.LogError("Error writing leased blob '{0}': {1}", context.ObjectId, (response as HttpWebResponse).StatusDescription);
                    throw new InvalidOperationException((response as HttpWebResponse).StatusDescription);
                }
            }
        }

        protected abstract T ReadObject(CloudBlob blob);

        protected abstract void WriteObject(CloudBlob blob, BlobRequestOptions options, T obj);

        protected abstract byte[] BinarizeObjectForStreaming(BlobProperties properties, T obj);
    }
}
