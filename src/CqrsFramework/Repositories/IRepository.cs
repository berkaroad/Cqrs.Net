using System;
using CqrsFramework.Entities;

namespace CqrsFramework.Repositories
{
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        TAggregateRoot Find(Guid id);

        TAggregateRoot Get(Guid id);

        void Save(TAggregateRoot aggregateRoot, string correlationId);
    }
}