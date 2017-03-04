using System;
using CqrsFramework.Ioc.Events;

namespace CqrsFramework.Ioc
{
    public interface IContainer
    {
        bool IsRegistered(Type serviceType);

        bool IsRegistered<TService>()
            where TService : class;

        IContainer RegisterType(Type implementationType, string serviceName = null, LifetimeScope life = LifetimeScope.Singleton);

        IContainer RegisterType(Type serviceType, Type implementationType, string serviceName = null, LifetimeScope life = LifetimeScope.Singleton);

        IContainer Register<TService, TImplementer>(string serviceName = null, LifetimeScope life = LifetimeScope.Singleton)
            where TService : class
            where TImplementer : class, TService;

        IContainer RegisterInstance<TService>(TService instance, string serviceName = null)
            where TService : class;

        TService Resolve<TService>()
            where TService : class;

        object Resolve(Type serviceType);

        bool TryResolve<TService>(out TService instance)
            where TService : class;

        bool TryResolve(Type serviceType, out object instance);

        TService ResolveNamed<TService>(string serviceName)
            where TService : class;

        object ResolveNamed(string serviceName, Type serviceType);

        bool TryResolveNamed(string serviceName, Type serviceType, out object instance);

        event ServiceTypeRegisteringEventHandler OnServiceTypeRegistering;

        event ServiceInstanceRegisteringEventHandler OnServiceInstanceRegistering;
    }
}
