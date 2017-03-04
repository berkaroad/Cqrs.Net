using System;
using Xunit;
using CqrsFramework.Ioc;
using System.Reflection;
using CqrsFramework.Configuration;

namespace CqrsFramework.Tests
{
    public class IocTests
    {
        #region datas

        public interface IUser
        {
            void method1();
            string method2();
            void method3(int parameter1);
            string method4(int parameter1, string parameter2, Type parameter3);
            string method4<TService>(int parameter1, string parameter2) where TService : class;
            void method5();

            string property1 { get; set; }

            event EventHandler event1;
        }

        [ServiceTypeConfig]
        public class Supplier : IUser
        {
            public void method1()
            {
                Console.WriteLine("Supplier.method1");
            }

            public string method2()
            {
                return "Supplier.method2";
            }
            public void method3(int parameter1)
            {
                Console.WriteLine("Supplier.method3:" + parameter1.ToString());
            }
            public string method4(int parameter1, string parameter2, Type parameter3)
            {
                return "Supplier.method4:" + parameter1.ToString() + ", " + parameter2 + ", " + parameter3.FullName;
            }
            public string method4<TService>(int parameter1, string parameter2)
                 where TService : class
            {
                return "GenericMethod:" + method4(parameter1, parameter2, typeof(TService));
            }
            public void method5()
            {
                event1.Invoke(this, new EventArgs());
            }

            public string property1
            {
                get { return "Supplier.property1:get"; }
                set { Console.WriteLine("Supplier.property1:set:" + value); }
            }

            public event EventHandler event1;
        }

        public class Saller { }

        #endregion

        public IocTests()
        {
            ConfigurationManager.Instance.UseAutofac();
            IocContainer.RegisterByAssembly(typeof(IocTests).GetTypeInfo().Assembly);
        }

        [Fact]
        public void Test1()
        {
            var resolver = IocContainer.Instance.Resolve<IIocResolver>();
            var user1 = resolver.Resolve<IUser>();

            user1.method1();
            Console.WriteLine(user1.method2());
            user1.method3(82);
            Console.WriteLine(user1.method4(53, "hello world", typeof(Saller)));
            Console.WriteLine(user1.method4<Saller>(53, "hello world"));

            user1.property1 = "Hi jerry";
            Console.WriteLine(user1.property1);

            user1.event1 += (sender, e) =>
            {
                Console.WriteLine("event1 subscriber01:" + e.GetType().FullName);
            };
            user1.event1 += (sender, e) =>
            {
                Console.WriteLine("event1 subscriber02:" + e.GetType().FullName);
            };

            var user2 = resolver.Resolve<IUser>();
            user2.method5();
        }
    }
}
