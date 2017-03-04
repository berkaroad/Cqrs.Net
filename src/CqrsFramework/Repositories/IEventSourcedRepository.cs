using CqrsFramework.EventSourcing;

namespace CqrsFramework.Repositories
{
    public interface IEventSourcedRepository<TEventSourced> : IRepository<TEventSourced>
        where TEventSourced : IEventSourced
    {
        
    }
}