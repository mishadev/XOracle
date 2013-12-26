using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface IBinarySerializer
    {
        Task<byte[]> ToBinary(object graph);

        Task<object> FromBinary(byte[] buffer);
    }
}
