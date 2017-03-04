using System.Collections.Generic;
using CqrsFramework.Entities;

namespace CqrsFramework.EventSourcing
{
    public interface IEventSourced : IAggregateRoot
    {
        int Version { get; }

        IEnumerable<IVersionedEvent> Events { get; }
    }
}
