using System.Collections.Concurrent;

namespace CqrsFramework.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private static readonly IConfigurationManager _instance = new ConfigurationManager();
        private ConcurrentDictionary<string, object> _configData = new ConcurrentDictionary<string, object>();

        private ConfigurationManager() { }

        public static IConfigurationManager Instance
        {
            get { return _instance; }
        }

        public object Get(string key)
        {
            object result = null;
            _configData.TryGetValue(key, out result);
            return result;
        }

        public void Set(string key, object value, bool canOverride)
        {
            if (canOverride)
            {
                _configData.AddOrUpdate(key, value, (k, v) => value);
            }
            else
            {
                _configData.TryAdd(key, value);
            }
        }
    }
}