/**
 *   Copyright (c) Rich Hickey. All rights reserved.
 *   The use and distribution terms for this software are covered by the
 *   Eclipse Public License 1.0 (http://opensource.org/licenses/eclipse-1.0.php)
 *   which can be found in the file epl-v10.html at the root of this distribution.
 *   By using this software in any fashion, you are agreeing to be bound by
 * 	 the terms of this license.
 *   You must not remove this notice, or any other, from this software.
 **/

/**
 *   Author: David Miller
 **/

using System.Linq.Expressions;
using System.Reflection;

namespace clojure.lang.Runtime.Binding
{
    static class BindingHelpers
    {
        // TODO: I think I can get rid of this now
        static readonly PropertyInfo Property_ClojureContext_Default = typeof(ClojureContext).GetProperty("Default");
        static readonly Expression _contextExpr = Expression.Property(null,Property_ClojureContext_Default);

        public static readonly MethodInfo Method_ClojureContext_GetDefault = typeof(ClojureContext).GetMethod("get_Default");

        internal static Expression CreateBinderStateExpression()
        {
            return _contextExpr;
        }
    }
}
