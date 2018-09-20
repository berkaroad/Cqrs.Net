using System;
using System.Linq;
using System.Reflection;

namespace CqrsFramework.DynamicProxy.Emitters
{
    public abstract class AbstractProxyMemberEmitter : IMemberEmitter
    {
        protected IProxyTypeGeneratorInfo _typeGeneratorInfo;

        protected AbstractProxyMemberEmitter(IProxyTypeGeneratorInfo typeGeneratorInfo)
        {
            _typeGeneratorInfo = typeGeneratorInfo;
        }

        public abstract void Emit(MemberInfo memberInfo);

        public static IInterceptor[] CreateInterceptors(Type[] interceptorTypes)
        {
            return interceptorTypes == null || interceptorTypes.Length == 0 ? null : interceptorTypes.Select(m => (IInterceptor)Activator.CreateInstance(m)).ToArray();
        }
    }
}