using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure.Utils
{
    public class NetBinarySerializer : IBinarySerializer
    {
        public async Task<byte[]> ToBinary(object graph)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var serializer = new BinaryFormatter();

                serializer.Serialize(stream, graph);

                return stream.ToArray();
            }
        }

        public async Task<object> FromBinary(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Position = 0;

                var serializer = new BinaryFormatter();
                return serializer.Deserialize(stream);
            }
        }
    }
}
