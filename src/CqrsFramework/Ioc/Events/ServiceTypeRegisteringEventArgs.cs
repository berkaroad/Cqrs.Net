using System;

namespace CqrsFramework.Ioc.Events
{
    public class ServiceTypeRegisteringEventArgs : EventArgs
    {
        public ServiceTypeRegisteringEventArgs(Type serviceType, Type implementationType, string serviceName)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            ServiceName = serviceName;
            Life = LifetimeScope.Singleton;
        }

        public Type ServiceType { get; private set; }

        public Type ImplementationType { get; private set; }

        internal Type NewImplementationType { get; private set; }

        public string ServiceName { get; private set; }

        public LifetimeScope Life { get; private set; }

        public void SetNewImplementationType(Type newImplementationType)
        {
            NewImplementationType = newImplementationType;
        }
    }
}
