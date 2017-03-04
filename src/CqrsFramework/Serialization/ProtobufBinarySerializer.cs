using System;
using System.IO;

namespace CqrsFramework.Serialization
{
    public class ProtobufBinarySerializer : IBinarySerializer
    {
        public void Serialize(Stream writer, object graph)
        {
            ProtoBuf.Serializer.NonGeneric.Serialize(writer, graph);
        }

        public object Deserialize(Type type, Stream reader)
        {
            return ProtoBuf.Serializer.NonGeneric.Deserialize(type, reader);
        }
    }
}