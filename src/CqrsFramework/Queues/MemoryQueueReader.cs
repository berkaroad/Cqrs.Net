using System.Collections.Concurrent;
using System.Threading;

namespace CqrsFramework.Queues
{
    public sealed class MemoryQueueReader : IQueueReader
    {
        private readonly BlockingCollection<byte[]>[] _queues;
        private readonly string[] _names;

        public MemoryQueueReader(BlockingCollection<byte[]>[] queues, string[] names)
        {
            _queues = queues;
            _names = names;
        }

        public void InitIfNeeded()
        {

        }

        public void AckMessage(MessageTransportContext message)
        {

        }

        public bool TakeMessage(CancellationToken token, out MessageTransportContext context)
        {
            while (!token.IsCancellationRequested)
            {
                // if incoming message is delayed and in future -> push it to the timer queue.
                // timer will be responsible for publishing back.
                byte[] envelope;
                var result = BlockingCollection<byte[]>.TakeFromAny(_queues, out envelope);
                if (result >= 0)
                {
                    context = new MessageTransportContext(result, envelope, _names[result]);
                    return true;
                }
            }
            context = null;
            return false;
        }

        public void TryNotifyNack(MessageTransportContext context)
        {
            var id = (int)context.TransportMessage;
            _queues[id].Add(context.Unpacked);
        }
    }
}