using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CqrsFramework.DynamicProxy.Emitters
{
    public class ProxyTypeGenerator : IProxyTypeGeneratorInfo
    {
        private Dictionary<string, FieldBuilder> _fieldBuilders = new Dictionary<string, FieldBuilder>();
        private Type _proxiedInterfaceType;
        private Type _cachedType;

        public ProxyTypeGenerator(ModuleBuilder moduleBuilder, Type proxiedInterfaceType, Type proxiedType, string classNameSuffix)
        {
            _proxiedInterfaceType = proxiedInterfaceType;
            ProxiedType = proxiedType;
            Builder = moduleBuilder.DefineType($"{proxiedType.Name}__{classNameSuffix}", TypeAttributes.Public | TypeAttributes.Sealed, null, new Type[]
            {
                proxiedInterfaceType
            });

            var proxiedObjField = Builder.DefineField(Consts.PROXIED_OBJECT_FIELD_NAME, proxiedInterfaceType, FieldAttributes.Private);
            AddField(proxiedObjField);
            var interceptorTypesField = Builder.DefineField(Consts.INTERCEPTOR_TYPES_FIELD_NAME, typeof(Type[]), FieldAttributes.Public | FieldAttributes.Static);
            AddField(interceptorTypesField);
        }

        public Type ProxiedType { get; private set; }

        public TypeBuilder Builder { get; private set; }

        public FieldBuilder GetField(string fieldName)
        {
            return _fieldBuilders[fieldName];
        }

        public void AddField(FieldBuilder field)
        {
            _fieldBuilders.Add(field.Name, field);
        }

        private bool IsTypeArrayEqual(Type[] array1, Type[] array2)
        {
            if (array1 == null && array2 == null)
                return true;
            if (array1 == null && array2 != null)
                return false;
            if (array1 != null && array2 == null)
                return false;
            if (array1.Length != array2.Length)
                return false;
            for (var i = 0; i < array1.Length; i++)
            {
                if (array1[i].IsGenericType && array2[i].GetTypeInfo().IsGenericType)
                {
                    if (!array1[i].GetGenericTypeDefinition().Equals(array2[i].GetGenericTypeDefinition()))
                        return false;
                }
                else if (!array1[i].Equals(array2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsMethodSignutureEqual(MethodInfo method1, MethodInfo method2)
        {
            return method1.Name == method2.Name
                && method1.GetGenericArguments().Length == method2.GetGenericArguments().Length
                && IsTypeArrayEqual(method1.GetParameters().Select(m => m.ParameterType).ToArray(), method2.GetParameters().Select(m => m.ParameterType).ToArray());
        }

        private List<MethodInfo> AddEvents(Type interfaceType, Type proxiedType)
        {
            var eventMethodList = new List<MethodInfo>();
            foreach (var interfaceEventInfo in interfaceType.GetEvents())
            {
                if (interfaceEventInfo.AddMethod != null)
                {
                    eventMethodList.Add(interfaceEventInfo.AddMethod);
                }
                if (interfaceEventInfo.RemoveMethod != null)
                {
                    eventMethodList.Add(interfaceEventInfo.RemoveMethod);
                }
                new ProxyEventEmitter(this).Emit(proxiedType.GetEvent(interfaceEventInfo.Name));
            }
            return eventMethodList;
        }

        List<MethodInfo> AddProperties(Type interfaceType, Type proxiedType)
        {
            var propertyMethodList = new List<MethodInfo>();
            foreach (var interfacePropertyInfo in interfaceType.GetProperties())
            {
                if (interfacePropertyInfo.CanRead)
                {
                    propertyMethodList.Add(interfacePropertyInfo.GetMethod);
                }
                if (interfacePropertyInfo.CanWrite)
                {
                    propertyMethodList.Add(interfacePropertyInfo.SetMethod);
                }
                new ProxyPropertyEmitter(this).Emit(proxiedType.GetProperty(interfacePropertyInfo.Name, interfacePropertyInfo.PropertyType, interfacePropertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray()));
            }
            return propertyMethodList;
        }

        void AddMethods(Type interfaceType, Type proxiedType, IList<MethodInfo>  eventMethodList, IList<MethodInfo> propertyMethodList)
        {
            foreach (var interfaceMethodInfo in interfaceType.GetMethods())
            {
                if (!propertyMethodList.Contains(interfaceMethodInfo)
                    && !eventMethodList.Contains(interfaceMethodInfo))
                {
                    var methodInfo = proxiedType.GetMethods().FirstOrDefault(m => IsMethodSignutureEqual(m, interfaceMethodInfo));
                    if (methodInfo == null)
                    {
                        var baseType = proxiedType.BaseType;
                        while (baseType != null)
                        {
                            methodInfo = baseType.GetMethods().FirstOrDefault(m => IsMethodSignutureEqual(m, interfaceMethodInfo));
                            if (methodInfo != null) break;
                            baseType = baseType.BaseType;
                        }
                    }
                    new ProxyMethodEmitter(this).Emit(methodInfo);
                }
            }
        }

        public Type Generate()
        {
            if (_cachedType == null)
            {
                // constructor
                new ProxyConstructorEmitter(this).Emit(null);
                foreach (var cctor in ProxiedType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance))
                {
                    new ProxyConstructorEmitter(this).Emit(cctor);
                }

                AddMethods(_proxiedInterfaceType, ProxiedType, AddEvents(_proxiedInterfaceType, ProxiedType), AddProperties(_proxiedInterfaceType, ProxiedType));
                foreach(var interfaceType in _proxiedInterfaceType.GetInterfaces())
                {
                    AddMethods(interfaceType, ProxiedType, AddEvents(interfaceType, ProxiedType), AddProperties(interfaceType, ProxiedType));
                }

                _cachedType = Builder.CreateTypeInfo().AsType();
            }
            return _cachedType;
        }
    }
}
