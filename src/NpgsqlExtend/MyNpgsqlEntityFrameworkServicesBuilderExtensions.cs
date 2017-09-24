using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Query.Sql.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MyNpgsqlEntityFrameworkServicesBuilderExtensions
    {
        /// <summary>
        ///     <para>
        ///         Adds the services required by the Npgsql database provider for Entity Framework
        ///         to an <see cref="IServiceCollection" />. You use this method when using dependency injection
        ///         in your application, such as with ASP.NET. For more information on setting up dependency
        ///         injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         You only need to use this functionality when you want Entity Framework to resolve the services it uses
        ///         from an external <see cref="IServiceCollection" />. If you are not using an external
        ///         <see cref="IServiceCollection" /> Entity Framework will take care of creating the services it requires.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///         public void ConfigureServices(IServiceCollection services)
        ///         {
        ///             var connectionString = "connection string to database";
        ///
        ///             services
        ///                 .AddEntityFrameworkSqlServer()
        ///                 .AddDbContext&lt;MyContext&gt;(options => options.UseNpgsql(connectionString));
        ///         }
        ///     </code>
        /// </example>
        /// <param name="serviceCollection"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <returns>
        ///     A builder that allows further Entity Framework specific setup of the <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddEntityFrameworkMyNpgsql([NotNull] this IServiceCollection serviceCollection)
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            var builder = new EntityFrameworkRelationalServicesBuilder(serviceCollection)
                .TryAdd<IDatabaseProvider, DatabaseProvider<NpgsqlOptionsExtension>>()
                .TryAdd<IValueGeneratorCache>(p => p.GetService<INpgsqlValueGeneratorCache>())
                .TryAdd<IRelationalTypeMapper, NpgsqlEFTypeMapper>()
                .TryAdd<ISqlGenerationHelper, RelationalSqlGenerationHelper>()
                .TryAdd<IMigrationsAnnotationProvider, NpgsqlMigrationsAnnotationProvider>()
                .TryAdd<IRelationalValueBufferFactoryFactory, TypedRelationalValueBufferFactoryFactory>()
                .TryAdd<IConventionSetBuilder, NpgsqlConventionSetBuilder>()
                .TryAdd<IUpdateSqlGenerator, NpgsqlUpdateSqlGenerator>()
                .TryAdd<IModificationCommandBatchFactory, NpgsqlModificationCommandBatchFactory>()
                .TryAdd<IValueGeneratorSelector, NpgsqlValueGeneratorSelector>()
                .TryAdd<IRelationalConnection>(p => p.GetService<INpgsqlRelationalConnection>())
                .TryAdd<IMigrationsSqlGenerator, NpgsqlMigrationsSqlGenerator>()
                .TryAdd<IRelationalDatabaseCreator, NpgsqlDatabaseCreator>()
                .TryAdd<IHistoryRepository, NpgsqlHistoryRepository>()
                .TryAdd<IExecutionStrategyFactory, NpgsqlExecutionStrategyFactory>()
                .TryAdd<IQueryCompilationContextFactory, NpgsqlQueryCompilationContextFactory>()
                .TryAdd<IMemberTranslator, NpgsqlCompositeMemberTranslator>()
                .TryAdd<ICompositeMethodCallTranslator, MyNpgsqlCompositeMethodCallTranslator>()
                .TryAdd<IQuerySqlGeneratorFactory, MyNpgsqlQuerySqlGeneratorFactory>()
                .TryAdd<ISqlTranslatingExpressionVisitorFactory, MyNpgsqlSqlTranslatingExpressionVisitorFactory>()
                .TryAddProviderSpecificServices(b => b
                    .TryAddSingleton<INpgsqlValueGeneratorCache, NpgsqlValueGeneratorCache>()
                    .TryAddScoped<INpgsqlSequenceValueGeneratorFactory, NpgsqlSequenceValueGeneratorFactory>()
                    .TryAddScoped<INpgsqlRelationalConnection, NpgsqlRelationalConnection>());

            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}