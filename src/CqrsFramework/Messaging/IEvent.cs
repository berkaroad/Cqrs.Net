using System;

namespace CqrsFramework.Messaging
{
    public interface IEvent
    {
        Guid SourceId { get; }
    }
}