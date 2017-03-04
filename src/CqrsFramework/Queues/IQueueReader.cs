using System.Threading;

namespace CqrsFramework.Queues
{
    public interface IQueueReader
    {
        void InitIfNeeded();

        void AckMessage(MessageTransportContext message);

        bool TakeMessage(CancellationToken token, out MessageTransportContext context);

        void TryNotifyNack(MessageTransportContext context);
    }
}