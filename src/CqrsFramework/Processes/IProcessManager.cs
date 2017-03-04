using System;
using System.Collections.Generic;
using CqrsFramework.Messaging;

namespace CqrsFramework.Processes
{
    public interface IProcessManager
    {
        Guid Id { get; }

        bool Completed { get; }

        IEnumerable<Envelope<ICommand>> Commands { get; }
    }
}
