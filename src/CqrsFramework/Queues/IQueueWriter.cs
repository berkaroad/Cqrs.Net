namespace CqrsFramework.Queues
{
    public interface IQueueWriter
    {
        string Name { get; }

        void PutMessage(byte[] envelope);
    }
}