using System;

namespace CqrsFramework.Ioc
{
    public interface IIocResolver
    {
        bool IsRegistered(Type serviceType);

        bool IsRegistered<TService>()
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
    }
}
