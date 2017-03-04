using System;

namespace CqrsFramework.Sessions
{
    public interface ISession
    {
        Guid UserId { get; }

        string UserDisplayName { get; }
    }
}