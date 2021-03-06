using System;
using System.Linq.Expressions;
using System.Dynamic;
using Microsoft.Scripting.Actions.Calls;
using System.Reflection;

namespace clojure.lang.Runtime.Binding
{
    class MetaAFn : DynamicMetaObject, IInferableInvokable
    {

        public MetaAFn(Expression expression, AFn aFn)
            : base(expression, BindingRestrictions.Empty, aFn)
        {

        }

        public static bool FnHasArity(IFnArity fn, int i)
        {
            return fn.HasArity(i);
        }

        static readonly MethodInfo Method_MetaAFn_FnHasArity = typeof(MetaAFn).GetMethod("FnHasArity");

        public InferenceResult GetInferredType(Type delegateType, Type parameterType)
        {
            if (!delegateType.IsSubclassOf(typeof(Delegate)))
            {
                throw new InvalidOperationException();
            }

            MethodInfo invoke = delegateType.GetMethod("Invoke");
            ParameterInfo[] pis = invoke.GetParameters();

            if (Value is IFnArity fn && fn.HasArity(pis.Length))
            {
                return new InferenceResult(
                    typeof(object),
                    Restrictions.Merge(
                        BindingRestrictions.GetExpressionRestriction(
                            Expression.Call(
                                Method_MetaAFn_FnHasArity,
                                Expression.Convert(Expression, typeof(IFnArity)),
                                Expression.Constant(pis.Length)))));

                //  Below is what we should be returning.
                //  However, for some reason, the restriction fails,
                //    and our dynamic method call goes into an infinite loop trying to fallback again and again.

                //return new InferenceResult(
                //    typeof(object),
                //    Restrictions.Merge(
                //        BindingRestrictions.GetTypeRestriction(Expression, typeof(IFnArity)
                //        ).Merge(
                //        BindingRestrictions.GetExpressionRestriction(
                //            Expression.Call(
                //                Method_MetaAFn_FnHasArity,
                //                Expression.Convert(Expression, typeof(IFnArity)),
                //                Expression.Constant(pis.Length))))));
            }

            return null;
        }
    }
}
