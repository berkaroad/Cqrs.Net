using CqrsFramework.Repositories;

namespace CqrsFramework.EventSourcing
{
    public interface IEventSourcedRepository<TEventSourced> : IRepository<TEventSourced>
        where TEventSourced : IEventSourced
    {
        
    }
}