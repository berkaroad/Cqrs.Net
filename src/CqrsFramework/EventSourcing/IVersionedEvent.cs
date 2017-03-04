using CqrsFramework.Messaging;

namespace CqrsFramework.EventSourcing
{
    public interface IVersionedEvent : IEvent
    {
        int Version { get; }
    }
}
