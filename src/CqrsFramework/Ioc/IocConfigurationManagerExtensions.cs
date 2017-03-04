using CqrsFramework.Configuration;

namespace CqrsFramework.Ioc
{
    public static class IocConfigurationManagerExtensions
    {
        private const string CONFIG_KEY = "CqrsFramework.Ioc.IContainer";

        public static IConfigurationManager UseIocContainer(this IConfigurationManager manager, IContainer iocContainer)
        {
            if (!iocContainer.IsRegistered<IIocResolver>())
            {
                iocContainer.Register<IIocResolver, Internal.DefaultIocResolver>();
            }
            manager.Set(CONFIG_KEY, iocContainer, false);
            return manager;
        }

        public static IContainer GetIocContainer(this IConfigurationManager manager)
        {
            return (IContainer)manager.Get(CONFIG_KEY);
        }
    }
}