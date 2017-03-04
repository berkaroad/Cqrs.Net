using System.IO;

namespace CqrsFramework.Serialization
{
    public interface ITextSerializer
    {
        void Serialize(TextWriter writer, object graph);

        object Deserialize(TextReader reader);
    }
}
