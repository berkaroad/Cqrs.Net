using System;
using System.IO;

namespace CqrsFramework.Serialization
{
    public interface IBinarySerializer
    {
        void Serialize(Stream writer, object graph);

        object Deserialize(Type type, Stream reader);
    }
}