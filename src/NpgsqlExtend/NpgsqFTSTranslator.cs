using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class NpgsqlFTSTranslator : IMethodCallTranslator
    {
        /*static readonly MethodInfo IsMatch =
            typeof(Regex).GetRuntimeMethod(nameof(Regex.IsMatch), new[] { typeof(string), typeof(string) });
        */
        private static readonly MethodInfo _FTS
            = typeof(MyNpgsqlDbFunctionsExtensions).GetRuntimeMethod(
                nameof(MyNpgsqlDbFunctionsExtensions.FTS),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) });

        [CanBeNull]
        public virtual Expression Translate([NotNull] MethodCallExpression methodCallExpression)
        {
            Check.NotNull(methodCallExpression, nameof(methodCallExpression));
            // Regex.IsMatch(string, string)
            /*
            if (methodCallExpression.Method.Equals(IsMatch))
            {
                return new RegexMatchExpression(
                    methodCallExpression.Arguments[0],
                    methodCallExpression.Arguments[1],
                    RegexOptions.None
                );
            }
            */
            var arg1 = methodCallExpression.Arguments[1];
            var arg2 = methodCallExpression.Arguments[2];
            //if(methodCallExpression.Arguments[1].ToString().Equals(methodCallExpression.Arguments[2]))
            //if (arg1.Equals(arg2))
            if (Equals(methodCallExpression.Method, _FTS))
                return new FTSExpression(methodCallExpression.Arguments[1], methodCallExpression.Arguments[2]);
            else
                return null;
        }
    }
}