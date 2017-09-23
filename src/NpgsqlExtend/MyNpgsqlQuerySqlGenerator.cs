using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.Sql.Internal
{
    public class MyNpgsqlQuerySqlGenerator : NpgsqlQuerySqlGenerator
    {
        public MyNpgsqlQuerySqlGenerator(
            QuerySqlGeneratorDependencies dependencies,
            SelectExpression selectExpression)
            : base(dependencies, selectExpression)
        {
        }

        public virtual Expression VisitFTS(FTSExpression FullTextSeachExpression)
        {

            Visit(FullTextSeachExpression.Column);
            //Sql.Append(" = ");
            Sql.Append(" &@~ ");
            Visit(FullTextSeachExpression.Param);

            return FullTextSeachExpression;
        }
#if false
        protected override string GenerateOperator(Expression expression)
        {
            switch (expression.NodeType)
            {
                //.Where(i.Name == name) can not return;
                /*
                case ExpressionType.Equal:
                    if (expression.Type == typeof(string))
                        return " &@~ ";
                    goto default;
                    */
                case ExpressionType.Add:
                    if (expression.Type == typeof(string))
                        return " || ";
                    goto default;
                case ExpressionType.And:
                    if (expression.Type == typeof(bool))
                        return " AND ";
                    goto default;
                case ExpressionType.Or:
                    if (expression.Type == typeof(bool))
                        return " OR ";
                    goto default;
                default:
                    return base.GenerateOperator(expression);
            }
        }
#endif
    }
}
