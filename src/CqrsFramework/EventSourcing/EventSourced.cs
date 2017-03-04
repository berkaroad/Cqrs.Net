using System;
using System.Collections.Generic;

namespace CqrsFramework.EventSourcing
{
    public abstract class EventSourced : IEventSourced
    {
        private readonly List<IVersionedEvent> _pendingEvents = new List<IVersionedEvent>();

        protected EventSourced(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; private set; }

        public int Version { get; protected set; }

        public IEnumerable<IVersionedEvent> Events
        {
            get { return _pendingEvents; }
        }

        protected void LoadFrom(IEnumerable<IVersionedEvent> pastEvents)
        {
            foreach (var e in pastEvents)
            {
                Mutate(e);
                this.Version = e.Version;
            }
        }

        protected abstract void Mutate<TVersionedEvent>(TVersionedEvent @event) where TVersionedEvent : IVersionedEvent;

        protected void Update(VersionedEvent e)
        {
            e.SourceId = this.Id;
            e.Version = this.Version + 1;
            Mutate(e);
            this.Version = e.Version;
            this._pendingEvents.Add(e);
        }
    }
}
