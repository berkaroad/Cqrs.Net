using System.Collections.Generic;
using System.IO;
using CqrsFramework.Messaging;

namespace CqrsFramework
{
    public class StandardMetadataProvider : IMetadataProvider
    {
        public virtual IDictionary<string, string> GetMetadata(object payload)
        {
            var metadata = new Dictionary<string, string>();
            var type = payload.GetType();

            metadata[StandardMetadata.AssemblyName] = Path.GetFileNameWithoutExtension(type.AssemblyQualifiedName);
            metadata[StandardMetadata.FullName] = type.FullName;
            metadata[StandardMetadata.Namespace] = type.Namespace;
            metadata[StandardMetadata.TypeName] = type.Name;

            var e = payload as IEvent;
            if (e != null)
            {
                metadata[StandardMetadata.SourceId] = e.SourceId.ToString();
                metadata[StandardMetadata.Kind] = StandardMetadata.EventKind;
            }

            var c = payload as ICommand;
            if (c != null)
            {
                metadata[StandardMetadata.Kind] = StandardMetadata.CommandKind;
            }

            return metadata;
        }
    }
}