using System.Collections.Concurrent;

namespace CqrsFramework.Queues
{
    public sealed class MemoryQueueWriter : IQueueWriter
    {
        private readonly BlockingCollection<byte[]> _queue;

        public string Name { get; private set; }

        public MemoryQueueWriter(BlockingCollection<byte[]> queue, string name)
        {
            _queue = queue;
            Name = name;
        }

        public void PutMessage(byte[] envelope)
        {
            _queue.Add(envelope);
        }
    }
}