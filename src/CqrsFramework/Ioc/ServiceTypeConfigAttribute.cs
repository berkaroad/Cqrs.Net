using System;

namespace CqrsFramework.Ioc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ServiceTypeConfigAttribute : Attribute
    {
        public ServiceTypeConfigAttribute()
        {
            Scope = LifetimeScope.Singleton;
            ServiceTypes = null;
        }

        public LifetimeScope Scope { get; set; }

        public Type[] ServiceTypes { get; set; }
    }
}
