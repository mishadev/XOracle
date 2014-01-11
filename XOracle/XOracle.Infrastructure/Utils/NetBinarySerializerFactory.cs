using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure.Utils
{
    public class NetBinarySerializerFactory : IFactory<IBinarySerializer>
    {
        public IBinarySerializer Create()
        {
            return new NetBinarySerializer();
        }
    }
}
