using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CqrsFramework.DynamicProxy.Emitters
{
    public class ProxyTypeGenerator2 : IProxyTypeGeneratorInfo
    {
        private Dictionary<string, FieldBuilder> _fieldBuilders = new Dictionary<string, FieldBuilder>();
        private Type _proxiedInterfaceType;
        private Type _cachedType;

        public ProxyTypeGenerator2(ModuleBuilder moduleBuilder, Type proxiedInterfaceType, Type proxiedType)
        {
            _proxiedInterfaceType = proxiedInterfaceType;
            ProxiedType = proxiedType;
            Builder = moduleBuilder.DefineType(string.Format("{0}__DynamicProxy2", proxiedType.Name), TypeAttributes.Public | TypeAttributes.Sealed, null, new Type[]
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

        public Type Generate()
        {
            if (_cachedType == null)
            {
                // constructor
                new ProxyConstructorEmitter(this).Emit(null);

                // event
                var eventMethodList = new List<MethodInfo>();
                foreach (var interfaceEventInfo in _proxiedInterfaceType.GetEvents())
                {
                    if (interfaceEventInfo.AddMethod != null)
                    {
                        eventMethodList.Add(interfaceEventInfo.AddMethod);
                    }
                    if (interfaceEventInfo.RemoveMethod != null)
                    {
                        eventMethodList.Add(interfaceEventInfo.RemoveMethod);
                    }
                    new ProxyEventEmitter(this).Emit(ProxiedType.GetEvent(interfaceEventInfo.Name));
                }

                // property
                var propertyMethodList = new List<MethodInfo>();
                foreach (var interfacePropertyInfo in _proxiedInterfaceType.GetProperties())
                {
                    if (interfacePropertyInfo.CanRead)
                    {
                        propertyMethodList.Add(interfacePropertyInfo.GetMethod);
                    }
                    if (interfacePropertyInfo.CanWrite)
                    {
                        propertyMethodList.Add(interfacePropertyInfo.SetMethod);
                    }
                    new ProxyPropertyEmitter(this).Emit(ProxiedType.GetProperty(interfacePropertyInfo.Name, interfacePropertyInfo.PropertyType, interfacePropertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray()));
                }

                // method
                foreach (var interfaceMethodInfo in _proxiedInterfaceType.GetMethods())
                {
                    if (!propertyMethodList.Contains(interfaceMethodInfo)
                        && !eventMethodList.Contains(interfaceMethodInfo))
                    {
                        new ProxyMethodEmitter(this).Emit(ProxiedType.GetMethod(interfaceMethodInfo.Name, interfaceMethodInfo.GetParameters().Select(p => p.ParameterType).ToArray()));
                    }
                }

                _cachedType = Builder.CreateTypeInfo().AsType();
            }
            return _cachedType;
        }
    }
}
