namespace CqrsFramework.Queues
{
    public sealed class MessageTransportContext
    {
        public readonly object TransportMessage;
        public readonly byte[] Unpacked;
        public readonly string QueueName;

        public MessageTransportContext(object transportMessage, byte[] unpacked, string queueName)
        {
            TransportMessage = transportMessage;
            QueueName = queueName;
            Unpacked = unpacked;
        }
    }
}