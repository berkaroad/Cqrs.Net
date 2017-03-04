using System.Linq;
using System.Reflection;

namespace CqrsFramework.Ioc
{
    public class IocContainer
    {
        private static IContainer _instance;
        private static object _locker = new object();

        public static IContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = CqrsFramework.Configuration.ConfigurationManager.Instance.GetIocContainer();
                        }
                        return _instance;
                    }
                }
                return _instance;
            }
        }

        public static void RegisterByAssembly(Assembly assemblyToRegister)
        {
            if (assemblyToRegister == null)
            {
                throw new System.ArgumentNullException("assemblyToRegister");
            }
            foreach (var implType in assemblyToRegister.GetTypes())
            {
                var interfaces = implType.GetInterfaces().ToArray();
                var serviceTypeConfig = implType.GetTypeInfo().GetCustomAttribute<ServiceTypeConfigAttribute>();
                if (serviceTypeConfig != null)
                {
                    var serviceTypes = serviceTypeConfig.ServiceTypes;
                    if (serviceTypes == null || serviceTypes.Length == 0)
                    {
                        if (interfaces != null && interfaces.Length == 1)
                        {
                            serviceTypes = interfaces;
                        }
                        else
                        {
                            serviceTypes = null;
                        }
                    }

                    if (serviceTypes == null || serviceTypes.Length == 0)
                    {
                        if (!Instance.IsRegistered(implType))
                        {
                            Instance.RegisterType(implType, life: serviceTypeConfig.Scope);
                        }
                    }
                    else
                    {
                        foreach (var serviceType in serviceTypes)
                        {
                            if (serviceType != null && serviceType.GetTypeInfo().IsInterface && interfaces.Contains(serviceType) && !Instance.IsRegistered(serviceType))
                            {
                                Instance.RegisterType(serviceType, implType, life: serviceTypeConfig.Scope);
                            }
                        }
                    }
                }
            }
        }
    }
}