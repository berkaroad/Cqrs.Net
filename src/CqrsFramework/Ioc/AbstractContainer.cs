using System;
using CqrsFramework.Ioc.Events;

namespace CqrsFramework.Ioc
{
    public abstract class AbstractContainer : IContainer
    {
        public abstract bool IsRegistered(Type serviceType, string serviceName = null);

        public virtual bool IsRegistered<TService>(string serviceName = null) where TService : class
        {
            return IsRegistered(typeof(TService));
        }

        public abstract IContainer RegisterType(Type implementationType, string serviceName = null, LifetimeScope life = LifetimeScope.Singleton);

        public IContainer RegisterType(Type serviceType, Type implementationType, string serviceName = null, LifetimeScope life = LifetimeScope.Singleton)
        {
            if (OnServiceTypeRegistering != null)
            {
                var e = new ServiceTypeRegisteringEventArgs(serviceType, implementationType, serviceName);
                OnServiceTypeRegistering(this, e);
                if (e.NewImplementationType != null)
                {
                    implementationType = e.NewImplementationType;
                }
            }
            return InnerRegisterType(serviceType, implementationType, serviceName, life);
        }

        protected abstract IContainer InnerRegisterType(Type serviceType, Type implementationType, string serviceName = null, LifetimeScope life = LifetimeScope.Singleton);

        public virtual IContainer Register<TService, TImplementer>(string serviceName, LifetimeScope life)
            where TService : class
            where TImplementer : class, TService
        {
            return RegisterType(typeof(TService), typeof(TImplementer), serviceName, life);
        }

        public IContainer RegisterInstance<TService>(TService instance, string serviceName)
            where TService : class
        {
            if (OnServiceInstanceRegistering != null)
            {
                var e = new ServiceInstanceRegisteringEventArgs(typeof(TService), instance, serviceName);
                OnServiceInstanceRegistering(this, e);
                if (e.NewInstance != null)
                {
                    instance = (TService)e.NewInstance;
                }
            }
            return InnerRegisterInstance<TService>(instance, serviceName);
        }

        protected abstract IContainer InnerRegisterInstance<TService>(TService instance, string serviceName)
            where TService : class;

        public abstract object Resolve(Type serviceType);

        public virtual TService Resolve<TService>() where TService : class
        {
            return (TService)Resolve(typeof(TService));
        }

        public abstract object ResolveNamed(string serviceName, Type serviceType);

        public virtual TService ResolveNamed<TService>(string serviceName) where TService : class
        {
            return (TService)ResolveNamed(serviceName, typeof(TService));
        }

        public abstract bool TryResolve(Type serviceType, out object instance);

        public abstract bool TryResolve<TService>(out TService instance) where TService : class;

        public abstract bool TryResolveNamed(string serviceName, Type serviceType, out object instance);

        public event ServiceTypeRegisteringEventHandler OnServiceTypeRegistering;

        public event ServiceInstanceRegisteringEventHandler OnServiceInstanceRegistering;
    }
}
