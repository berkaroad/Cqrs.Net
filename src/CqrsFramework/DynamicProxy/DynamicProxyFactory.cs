using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CqrsFramework.DynamicProxy.Emitters;

namespace CqrsFramework.DynamicProxy
{
    public sealed class DynamicProxyFactory
    {
        private static ConcurrentDictionary<ProxyTypeIdentity, ProxyTypeWrapper> _proxyTypeDics = new ConcurrentDictionary<ProxyTypeIdentity, ProxyTypeWrapper>();

        public static ProxyTypeWrapper CreateProxyType(Type interfaceType, Type proxiedType, Type[] interceptorTypes)
        {
            var key = new ProxyTypeIdentity(interfaceType, proxiedType);
            var proxyType = _proxyTypeDics.AddOrUpdate(key, t =>
            {
                AssemblyName assemblyName = new AssemblyName(string.Format("{0}_{1}__DynamicAssembly", proxiedType.Name, interfaceType.Name));
                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                var moduleBuilder = assemblyBuilder.DefineDynamicModule(string.Format("{0}_{1}__DynamicModule", proxiedType.Name, interfaceType.Name));

                return new ProxyTypeWrapper
                {
                    ProxyType = new ProxyTypeGenerator(moduleBuilder, interfaceType, proxiedType).Generate(),
                    Proxy2Type = new ProxyTypeGenerator2(moduleBuilder, interfaceType, proxiedType).Generate(),
                    InterceptorTypes = interceptorTypes == null ? null : interceptorTypes.Where(i => typeof(IInterceptor).IsAssignableFrom(i)).Distinct().ToArray()
                };
            }, (t, originVal) =>
            {
                if (interceptorTypes != null)
                {
                    interceptorTypes = interceptorTypes.Where(i => typeof(IInterceptor).IsAssignableFrom(i)).ToArray();
                    List<Type> interceptorTypeList = new List<Type>(interceptorTypes);
                    if (_proxyTypeDics[key].InterceptorTypes != null)
                    {
                        interceptorTypeList.AddRange(_proxyTypeDics[key].InterceptorTypes);
                    }
                    originVal.InterceptorTypes = interceptorTypeList.Distinct().ToArray();
                }
                return originVal;
            });
            proxyType.ProxyType.GetField(Consts.INTERCEPTOR_TYPES_FIELD_NAME, BindingFlags.Public | BindingFlags.Static | BindingFlags.SetField).SetValue(null, proxyType.InterceptorTypes);
            proxyType.Proxy2Type.GetField(Consts.INTERCEPTOR_TYPES_FIELD_NAME, BindingFlags.Public | BindingFlags.Static | BindingFlags.SetField).SetValue(null, proxyType.InterceptorTypes);
            return proxyType;
        }

        public static object CreateProxy(Type interfaceType, object proxiedObj, Type[] interceptorTypes)
        {
            ProxyTypeWrapper proxyType = CreateProxyType(interfaceType, proxiedObj.GetType(), interceptorTypes);
            return Activator.CreateInstance(proxyType.Proxy2Type, proxiedObj);
        }

        public static TInterface CreateProxy<TInterface>(TInterface proxiedObj, Type[] interceptorTypes)
            where TInterface : class
        {
            return (TInterface)CreateProxy(typeof(TInterface), proxiedObj, interceptorTypes);
        }
    }

    public sealed class ProxyTypeWrapper
    {
        public Type ProxyType { get; set; }

        internal Type Proxy2Type { get; set; }

        public Type[] InterceptorTypes { get; set; }
    }

    public struct ProxyTypeIdentity
    {
        public ProxyTypeIdentity(Type interfaceType, Type proxiedType)
        {
            InterfaceType = interfaceType;
            ProxiedType = proxiedType;
        }

        public Type InterfaceType { get; private set; }

        public Type ProxiedType { get; private set; }
    }
}
