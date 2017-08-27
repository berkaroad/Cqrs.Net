using System;
using CqrsFramework.Entities;

namespace CqrsFramework.Repositories
{
    public class DapperRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        public TAggregateRoot Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public TAggregateRoot Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(TAggregateRoot aggregateRoot, string correlationId)
        {
            throw new NotImplementedException();
        }
    }
}
