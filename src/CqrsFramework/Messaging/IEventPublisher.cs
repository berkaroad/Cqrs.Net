using System.Collections.Generic;

namespace CqrsFramework.Messaging
{
    public interface IEventPublisher
    {
        IEnumerable<IEvent> Events { get; }
    }
}