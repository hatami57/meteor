using System.Data.Common;
using System.Threading.Tasks;

namespace Meteor.Database.Dapper
{
    public abstract class DbConnectionFactory : IDbConnectionFactory
    {
        protected string ConnectionString { get; set; }

        static DbConnectionFactory()
        {
            global::Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
        
        public DbConnectionFactory(string? connectionString = null)
        {
            ConnectionString = connectionString ?? EnvVars.DbUri;
        }

        public abstract Task<DbConnection> OpenNewConnectionAsync();
    }
}
