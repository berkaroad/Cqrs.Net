namespace CqrsFramework.DynamicProxy
{
    public interface IInterceptor
    {
        void Intercept(IInvocation invocation);
    }
}
