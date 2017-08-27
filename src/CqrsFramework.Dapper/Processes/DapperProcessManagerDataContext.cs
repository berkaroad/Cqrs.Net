using System;
using System.Linq.Expressions;

namespace CqrsFramework.Processes
{
    public class DapperProcessManagerDataContext<T> : IProcessManagerDataContext<T>
        where T : class, IProcessManager
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public T Find(Expression<Func<T, bool>> predicate, bool includeCompleted = false)
        {
            throw new NotImplementedException();
        }

        public void Save(T processManager)
        {
            throw new NotImplementedException();
        }
    }
}