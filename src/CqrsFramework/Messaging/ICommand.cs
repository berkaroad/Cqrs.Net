using System;

namespace CqrsFramework.Messaging
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}