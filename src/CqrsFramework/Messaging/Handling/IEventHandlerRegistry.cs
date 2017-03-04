namespace CqrsFramework.Messaging.Handling
{
    public interface IEventHandlerRegistry
    {
        void Register(IEventHandler handler);
    }
}
