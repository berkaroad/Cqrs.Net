using System.Reflection;

namespace CqrsFramework.DynamicProxy
{
    public interface IInvocation
    {
        MemberTypes MemberType { get; }

        string MemberName { get; }

        MemberOperateTypes MemberOperateType { get; }

        object InvocationTarget { get; }

        MethodInfo MethodInvocationTarget { get; }

        object Proxy { get; }

        MethodInfo Method { get; }

        object[] Arguments { get; }

        WrapperObject ReturnValue { get; }

        void Proceed();
    }

    public class WrapperObject
    {
        public WrapperObject() { }

        public object Value { get; set; }
    }

    public enum MemberOperateTypes
    {
        None = 0,

        Get = 1,

        Set = 2,

        OnAdd = 3,

        OnRemove = 4,

        OnRaise = 5
    }
}
