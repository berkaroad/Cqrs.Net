using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CqrsFramework.DynamicProxy.Emitters
{
    public class ProxyConstructorEmitter : AbstractProxyMemberEmitter
    {
        public ProxyConstructorEmitter(IProxyTypeGeneratorInfo typeGenerator) : base(typeGenerator) { }

        public override void Emit(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                var cctor = _typeGeneratorInfo.Builder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[] { _typeGeneratorInfo.ProxiedType });
                var proxiedObjField = _typeGeneratorInfo.GetField(Consts.PROXIED_OBJECT_FIELD_NAME);
                var generator = cctor.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Stfld, proxiedObjField);
                generator.Emit(OpCodes.Ret);
            }
            else
            {
                var proxiedTypeConstructorInfo = memberInfo as ConstructorInfo;
                Type[] paramTypes = proxiedTypeConstructorInfo.GetParameters().Select(m => m.ParameterType).ToArray();
                var cctor = _typeGeneratorInfo.Builder.DefineConstructor(proxiedTypeConstructorInfo.Attributes, proxiedTypeConstructorInfo.CallingConvention, paramTypes);
                var proxiedObjField = _typeGeneratorInfo.GetField(Consts.PROXIED_OBJECT_FIELD_NAME);
                var generator = cctor.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                for (var i = 0; i < paramTypes.Length; i++)
                {
                    generator.Emit(OpCodes.Ldarg, i + 1);
                }
                generator.Emit(OpCodes.Newobj, proxiedTypeConstructorInfo);
                generator.Emit(OpCodes.Stfld, proxiedObjField);
                generator.Emit(OpCodes.Ret);
            }
        }
    }
}
