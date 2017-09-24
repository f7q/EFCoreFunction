using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.Sql.Internal
{
    public class MyNpgsqlQuerySqlGeneratorFactory : NpgsqlQuerySqlGeneratorFactory
    {
        public MyNpgsqlQuerySqlGeneratorFactory([NotNull] QuerySqlGeneratorDependencies dependencies)
            : base(dependencies)
        {
        }

        public override IQuerySqlGenerator CreateDefault(SelectExpression selectExpression)
            => new MyNpgsqlQuerySqlGenerator(
                Dependencies, selectExpression);
    }
}