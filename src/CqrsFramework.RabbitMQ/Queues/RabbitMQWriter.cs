using System;

namespace CqrsFramework.Queues
{
    public class RabbitMQWriter : IQueueWriter
    {
        public string Name { get; private set; }

        public void PutMessage(byte[] envelope)
        {
            throw new NotImplementedException();
        }
    }
}