using System;

namespace CqrsFramework.Ioc.Events
{
    public class ServiceInstanceRegisteringEventArgs : EventArgs
    {
        public ServiceInstanceRegisteringEventArgs(Type serviceType, object instance, string serviceName)
        {
            ServiceType = serviceType;
            Instance = instance;
            ServiceName = serviceName;
            Life = LifetimeScope.Singleton;
        }

        public Type ServiceType { get; private set; }

        public object Instance { get; private set; }

        internal object NewInstance { get; private set; }

        public string ServiceName { get; private set; }

        public LifetimeScope Life { get; private set; }

        public void SetNewInstance(object newInstance)
        {
            NewInstance = newInstance;
        }
    }
}
