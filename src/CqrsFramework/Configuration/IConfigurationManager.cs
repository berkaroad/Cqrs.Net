namespace CqrsFramework.Configuration
{
    public interface IConfigurationManager
    {
        object Get(string key);

        void Set(string key, object value, bool canOverride);
    }
}