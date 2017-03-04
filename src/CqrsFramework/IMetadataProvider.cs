using System.Collections.Generic;

namespace CqrsFramework
{
    public interface IMetadataProvider
    {
        IDictionary<string, string> GetMetadata(object payload);
    }
}