using System;

namespace CqrsFramework.Entities
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
