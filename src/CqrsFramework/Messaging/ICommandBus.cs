using System.Collections.Generic;

namespace CqrsFramework.Messaging
{
    public interface ICommandBus
    {
        void Send(Envelope<ICommand> command);

        void Send(IEnumerable<Envelope<ICommand>> commands);
    }
}