using System.IO;

namespace CqrsFramework.Serialization
{
    public static class BinarySerializerExtensions
    {
        public static byte[] Serialize<T>(this IBinarySerializer serializer, T data)
        {
            using (var writer = new MemoryStream())
            {
                serializer.Serialize(writer, data);
                return writer.ToArray();
            }
        }

        public static T Deserialize<T>(this IBinarySerializer serializer, byte[] serialized)
        {
            using (var reader = new MemoryStream(serialized))
            {
                return (T)serializer.Deserialize(typeof(T), reader);
            }
        }
    }
}
