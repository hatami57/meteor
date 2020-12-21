using Meteor.Database.Sql.SqlDialect;
using Microsoft.Extensions.DependencyInjection;

namespace Meteor.Database.Dapper.Operations.Db
{
    public static class DbOperationExtensions
    {
        public static IServiceCollection AddDbOperation<TDbConnectionFactory, TSqlDialect>(this IServiceCollection services)
            where TDbConnectionFactory : class, IDbConnectionFactory
            where TSqlDialect : ISqlDialect, new()
        {
            services.AddSingleton<IDbConnectionFactory, TDbConnectionFactory>();
            services.AddSingleton<ISqlFactory, SqlFactory<TSqlDialect>>();
            services.AddScoped<LazyDbConnection>();

            return services;
        }
    }
}