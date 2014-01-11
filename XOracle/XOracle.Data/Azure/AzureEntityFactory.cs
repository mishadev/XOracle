using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Linq.Expressions;
using XOracle.Domain.Core;

namespace XOracle.Data.Azure
{
    public static class AzureEntityFactory
    {
        public static TAzureEntity ToAzureEntity<TAzureEntity, TEntity>(TEntity entity)
            where TAzureEntity : TableServiceEntity
            where TEntity : Entity
        {
            return null;
        }

        public static TEntity FromAzureEntity<TAzureEntity, TEntity>(TAzureEntity entity)
            where TAzureEntity : TableServiceEntity
            where TEntity : Entity
        {
            return null;
        }

        public static Expression<Func<TAzureEntity, bool>> ToAzureFilter<TAzureEntity, TEntity>(Expression<Func<TEntity, bool>> filter)
            where TAzureEntity : TableServiceEntity
            where TEntity : Entity
        {
            return null;
        }
    }
}
