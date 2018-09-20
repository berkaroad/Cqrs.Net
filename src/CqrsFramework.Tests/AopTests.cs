using System;
using Xunit;
using CqrsFramework.Ioc;
using System.Reflection;
using CqrsFramework.DynamicProxy;
using CqrsFramework.Configuration;

namespace CqrsFramework.Tests
{
    public class AopTests
    {
        #region datas

        public interface IEater<T>
        {
            void Eat(T t);
        }

        public class Rice
        {

        }

        public interface IUser : IEater<Rice>
        {
            void method1();
            int method2();
            void method3(int parameter1);
            string method4(int parameter1, string parameter2, Type parameter3);
            string method4<TService>(int parameter1, string parameter2) where TService : class;
            void method5();

            string property1 { get; set; }
            int property2 { get; set; }

            event EventHandler event1;
        }

        [ServiceTypeConfig(ServiceTypes = new Type[]{typeof(IUser)})]
        public class Supplier : IUser
        {
            public Supplier()
            {
                Console.WriteLine("Supplier.cctor");
            }
            public void method1()
            {
                Console.WriteLine("Supplier.method1");
            }

            public int method2()
            {
                return 2;
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

            public int property2 { get; set; }

            public event EventHandler event1;

            public void Eat(Rice rice)
            {
                Console.WriteLine("Supplier eat rice");
            }
        }

        public class Saller : IUser
        {
            public string property1 { get; set; }
            public int property2 { get; set; }

            public event EventHandler event1;

            public void method1()
            {
                // throw new NotImplementedException();
            }

            public int method2()
            {
                return 0;
                // throw new NotImplementedException();
            }

            public void method3(int parameter1)
            {
                // throw new NotImplementedException();
            }

            public string method4(int parameter1, string parameter2, Type parameter3)
            {
                return "";
                // throw new NotImplementedException();
            }

            public string method4<TService>(int parameter1, string parameter2) where TService : class
            {
                return "";
                // throw new NotImplementedException();
            }

            public void method5()
            {
                // throw new NotImplementedException();
            }

            public void Eat(Rice rice)
            {
                Console.WriteLine("Saller eat rice");
            }
        }

        public class UnitOfWorkInterceptor : IInterceptor
        {
            public void Intercept(IInvocation invocation)
            {
                if (invocation.MemberType == MemberTypes.Method)
                {
                    Console.WriteLine("UnitOfWorkInterceptor before invoke method");
                    invocation.Proceed();
                    Console.WriteLine("UnitOfWorkInterceptor after invoke method");
                }
                else
                {
                    invocation.Proceed();
                }
            }
        }
        public class PerformanceCounterInterceptor : IInterceptor
        {
            public void Intercept(IInvocation invocation)
            {
                if (invocation.MemberType == MemberTypes.Method)
                {
                    Console.WriteLine("PerformanceCounterInterceptor before invoke method");
                    invocation.Proceed();
                    Console.WriteLine("PerformanceCounterInterceptor after invoke method");
                }
                else if (invocation.MemberType == MemberTypes.Property)
                {
                    Console.WriteLine("PerformanceCounterInterceptor before invoke property");
                    // if (invocation.MemberName == "property2")
                    // {
                    //    invocation.ReturnValue.Value = 23;
                    // }
                    invocation.Proceed();
                    Console.WriteLine("PerformanceCounterInterceptor after invoke property");
                }
                else
                {
                    invocation.Proceed();
                }
            }
        }

        #endregion


        public AopTests()
        {
            ConfigurationManager.Instance.UseAutofac();
            IocContainer.Instance.OnServiceTypeRegistering += (sender, e) =>
            {
                if (e.ServiceType == typeof(IUser))
                {
                    e.SetNewImplementationType(DynamicProxyFactory.CreateProxyType(e.ServiceType
                        , e.ImplementationType
                        , new Type[] {
                            typeof(UnitOfWorkInterceptor)
                            ,typeof(PerformanceCounterInterceptor) }).ProxyType);
                }
            };
            IocContainer.Instance.OnServiceInstanceRegistering += (sender, e) =>
            {
                if (e.ServiceType == typeof(IUser))
                {
                    e.SetNewInstance(DynamicProxyFactory.CreateProxy(e.ServiceType
                        , e.Instance
                        , new Type[] {
                            typeof(UnitOfWorkInterceptor)
                            ,typeof(PerformanceCounterInterceptor) }));
                }
            };
            IocContainer.RegisterByAssembly(typeof(AopTests).GetTypeInfo().Assembly);
        }

        [Fact]
        public void Test1()
        {
            var obj1 = DynamicProxyFactory.CreateProxy<IUser>(new Saller(), null);
            obj1.property2 = -9;
            Assert.Equal(-9, obj1.property2);
            obj1.Eat(new Rice());

            var obj2 = DynamicProxyFactory.CreateProxyType(typeof(IUser), typeof(Supplier), null).ProxyType.GetConstructor(Type.EmptyTypes).Invoke(null);
            
            var resolver = IocContainer.Instance.Resolve<IIocResolver>();
            var user1 = resolver.Resolve<IUser>();

            var val1 = user1.method2();
            var val2 = user1.property1;
            user1.property2 = -9;
            Assert.Equal(-9, user1.property2);
            user1.Eat(new Rice());
        }
    }
}