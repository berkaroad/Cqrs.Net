using System.Reflection;

namespace CqrsFramework.DynamicProxy.Emitters
{
    public interface IMemberEmitter
    {
        void Emit(MemberInfo memberInfo);
    }
}
