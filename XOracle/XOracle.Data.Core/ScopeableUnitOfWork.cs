using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Data.Core
{
    public class ScopeableUnitOfWork : IScopeable<IUnitOfWork>
    {
        private static IDictionary<IUnitOfWork, int> _global = new Dictionary<IUnitOfWork, int>();

        private int _depth = 0;
        private IUnitOfWork _unitOfWork;

        public ScopeableUnitOfWork(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            if (!_global.ContainsKey(this._unitOfWork))
                _global.Add(this._unitOfWork, 0);

            this.Increment();
        }

        private void Increment()
        {
            while (true)
            {
                if (Interlocked.Exchange(ref this._depth, _global[this._unitOfWork]) == _global[this._unitOfWork])
                {
                    _global[this._unitOfWork] = Interlocked.Increment(ref this._depth);
                    break;
                }
            }
        }

        private void Decrement()
        {
            while (true)
            {
                if (Interlocked.Exchange(ref this._depth, _global[this._unitOfWork]) == _global[this._unitOfWork])
                {
                    _global[this._unitOfWork] = Interlocked.Decrement(ref this._depth);
                    break;
                }
            }
        }

        public void Dispose()
        {
            this.Decrement();
            if (this._depth < 1)
                this._unitOfWork.Commit().GetAwaiter().GetResult();
        }

        public async Task Rollback()
        {
            if (this._depth <= 1)
                await this._unitOfWork.Rollback();
        }
    }
}
