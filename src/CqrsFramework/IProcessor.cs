namespace CqrsFramework
{
    public interface IProcessor
    {
        void Start();

        void Stop();
    }
}
