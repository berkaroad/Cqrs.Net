using CqrsFramework.Configuration;

namespace CqrsFramework.Ioc
{
    public static class AutofacConfigurationManagerExtensions
    {
        public static IConfigurationManager UseAutofac(this IConfigurationManager manager)
        {
            return manager.UseIocContainer(new AutofacContainer());
        }
    }
}