using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private IDictionarySetUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IDictionarySetUnitOfWork))
                throw new InvalidCastException("unitOfWork");

            this._unitOfWork = (IDictionarySetUnitOfWork)unitOfWork;
        }

        private async Task<IDictionary<Guid, TEntity>> GetSet()
        {
            return await this._unitOfWork.CreateSet<TEntity>();
        }

        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        public async Task Add(TEntity item)
        {
            var set = await this.GetSet();
            item.EnsureIdentity();

            Validate(item);

            set.Add(item.Id, item);
        }

        public async Task Add(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
               await this.Add(item);
        }

        public async Task Remove(TEntity item)
        {
            var set = await this.GetSet();

            set.Remove(item.Id);
        }

        public async Task Remove(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
                await this.Remove(item);
        }

        public async Task Modify(TEntity item)
        {
            var set = await this.GetSet();

            Validate(item);

            set[item.Id] = item;
        }

        public async Task Modify(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
                await this.Modify(item);
        }

        public async Task<TEntity> Get(Guid id)
        {
            var set = await this.GetSet();

            TEntity value;
            set.TryGetValue(id, out value);

            return value;
        }

        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> filter)
        {
            var set = await this.GetSet();

            return set
                .Select(kvp => kvp.Value)
                .FirstOrDefault(filter.Compile());
        }

        public async Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            var set = await this.GetSet();

            return set
                .Select(kvp => kvp.Value)
                .Where(filter.Compile());
        }

        public void Dispose()
        {
            this._unitOfWork.Dispose();
        }

        private void Validate(TEntity item)
        {
            var validator = Factory<IValidator>.GetInstance();
            var validationErrors = validator.GetErrorMessages(item);

            if (validationErrors.Any())
                throw new InvalidOperationException("validation errors: " + string.Join(", ", validationErrors));
        }
    }
}
