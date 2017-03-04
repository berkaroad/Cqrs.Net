using System.Collections.Generic;

namespace CqrsFramework.Messaging
{
    public interface IEventBus
    {
        void Publish(Envelope<IEvent> @event);

        void Publish(IEnumerable<Envelope<IEvent>> events);
    }
}