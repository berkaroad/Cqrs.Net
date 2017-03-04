namespace CqrsFramework.Sessions
{
    public interface ISessionProvider
    {
        ISession GetSession();
    }
}