using System;

namespace CqrsFramework.EventSourcing
{
    public class DapperEventSourcedRepository<TEventSourced> : IEventSourcedRepository<TEventSourced>
        where TEventSourced : IEventSourced
    {
        public TEventSourced Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public TEventSourced Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(TEventSourced aggregateRoot, string correlationId)
        {
            throw new NotImplementedException();
        }
    }
}