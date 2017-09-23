using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class NpgsqlFTSTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _FTS
            = typeof(MyNpgsqlDbFunctionsExtensions).GetRuntimeMethod(
                nameof(MyNpgsqlDbFunctionsExtensions.FTS),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) });

        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            //Check.NotNull(methodCallExpression, nameof(methodCallExpression));

            if (Equals(methodCallExpression.Method, _FTS))
                return new FTSExpression(methodCallExpression.Arguments[1], methodCallExpression.Arguments[2]);
            else
                return null;
        }
    }
}