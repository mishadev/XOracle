using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XOracle.Domain.Core;

namespace XOracle.Data.Core
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
            set.Add(item.Id, item);
        }

        public async Task Remove(TEntity item)
        {
            var set = await this.GetSet();

            set.Remove(item.Id);
        }

        public async Task Modify(TEntity item)
        {
            var set = await this.GetSet();

            set[item.Id] = item;
        }

        public async Task<TEntity> Get(Guid id)
        {
            var set = await this.GetSet();

            TEntity value;
            set.TryGetValue(id, out value);

            return value;
        }

        public async Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter, int page, int size)
        {
            var set = await this.GetSet();

            return set
                .Skip(page * size)
                .Take(size)
                .Select(kvp => kvp.Value)
                .Where(filter.Compile());
        }

        public void Dispose()
        {
            this._unitOfWork.Dispose();
        }
    }
}
