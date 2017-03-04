using System;
using System.Linq;
using Autofac;

namespace CqrsFramework.Ioc
{
    public class AutofacContainer : CqrsFramework.Ioc.AbstractContainer
    {
        private Autofac.IContainer _container = null;

        public AutofacContainer()
        {
            _container = new ContainerBuilder().Build(Autofac.Builder.ContainerBuildOptions.ExcludeDefaultModules);
        }

        public override bool IsRegistered(Type serviceType)
        {
            return _container.IsRegistered(serviceType);
        }

        public override IContainer RegisterType(Type implementationType, string serviceName = null, LifetimeScope life = LifetimeScope.Singleton)
        {
            var builder = new ContainerBuilder();
            var registrationBuilder = builder.RegisterType(implementationType);
            if (serviceName != null)
            {
                registrationBuilder.Named(serviceName, implementationType);
            }
            if (life == CqrsFramework.Ioc.LifetimeScope.Singleton)
            {
                registrationBuilder.SingleInstance();
            }

            if (IsRegistered(implementationType))
            {
                _container.ComponentRegistry.Registrations.Where(r => r.Services.Any(s =>
                {
                    var ts = s as Autofac.Core.TypedService;
                    return ts != null && ts.ServiceType != implementationType;
                })).ToList().ForEach(r => builder.RegisterComponent(r));
                _container = builder.Build(Autofac.Builder.ContainerBuildOptions.ExcludeDefaultModules);
            }
            else
            {
                builder.Build().ComponentRegistry.Registrations.Where(r => r.Services.Any(s => s is Autofac.Core.TypedService)).ToList().ForEach(r => _container.ComponentRegistry.Register(r));
            }
            return this;
        }

        protected override IContainer InnerRegisterInstance<TService>(TService instance, string serviceName)
        {
            var builder = new ContainerBuilder();
            var registrationBuilder = builder.RegisterInstance(instance).As<TService>().SingleInstance();
            if (serviceName != null)
            {
                registrationBuilder.Named<TService>(serviceName);
            }

            if (IsRegistered<TService>())
            {
                _container.ComponentRegistry.Registrations.Where(r => r.Services.Any(s =>
                {
                    var ts = s as Autofac.Core.TypedService;
                    return ts != null && ts.ServiceType != typeof(TService);
                })).ToList().ForEach(r => builder.RegisterComponent(r));
                _container = builder.Build(Autofac.Builder.ContainerBuildOptions.ExcludeDefaultModules);
            }
            else
            {
                builder.Build().ComponentRegistry.Registrations.Where(r => r.Services.Any(s => s is Autofac.Core.TypedService)).ToList().ForEach(r => _container.ComponentRegistry.Register(r));
            }
            return this;
        }

        protected override IContainer InnerRegisterType(Type serviceType, Type implementationType, string serviceName = null, LifetimeScope life = LifetimeScope.Singleton)
        {
            var builder = new ContainerBuilder();
            var registrationBuilder = builder.RegisterType(implementationType).As(serviceType);
            if (serviceName != null)
            {
                registrationBuilder.Named(serviceName, serviceType);
            }
            if (life == CqrsFramework.Ioc.LifetimeScope.Singleton)
            {
                registrationBuilder.SingleInstance();
            }

            if (IsRegistered(serviceType))
            {
                _container.ComponentRegistry.Registrations.Where(r => r.Services.Any(s =>
                {
                    var ts = s as Autofac.Core.TypedService;
                    return ts != null && ts.ServiceType != serviceType;
                })).ToList().ForEach(r => builder.RegisterComponent(r));
                _container = builder.Build(Autofac.Builder.ContainerBuildOptions.ExcludeDefaultModules);
            }
            else
            {
                builder.Build().ComponentRegistry.Registrations.Where(r => r.Services.Any(s => s is Autofac.Core.TypedService)).ToList().ForEach(r => _container.ComponentRegistry.Register(r));
            }
            return this;
        }

        public override object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public override object ResolveNamed(string serviceName, Type serviceType)
        {
            return _container.ResolveNamed(serviceName, serviceType);
        }

        public override bool TryResolve(Type serviceType, out object instance)
        {
            return _container.TryResolve(serviceType, out instance);
        }

        public override bool TryResolve<TService>(out TService instance)
        {
            return _container.TryResolve<TService>(out instance);
        }

        public override bool TryResolveNamed(string serviceName, Type serviceType, out object instance)
        {
            return _container.TryResolveNamed(serviceName, serviceType, out instance);
        }
    }
}