using System;
using System.Threading;

namespace CqrsFramework.Queues
{
    public class RabbitMQReader : IQueueReader
    {
        public void AckMessage(MessageTransportContext message)
        {
            throw new NotImplementedException();
        }

        public void InitIfNeeded()
        {
            throw new NotImplementedException();
        }

        public bool TakeMessage(CancellationToken token, out MessageTransportContext context)
        {
            throw new NotImplementedException();
        }

        public void TryNotifyNack(MessageTransportContext context)
        {
            throw new NotImplementedException();
        }
    }
}
