using System.Data.Common;
using System.Threading.Tasks;
using Meteor.Database.Dapper.Sqlite.TypeHandlers;
using Microsoft.Data.Sqlite;

namespace Meteor.Database.Dapper.Sqlite
{
    public class SqliteDbConnectionFactory : DbConnectionFactory
    {
        public SqliteDbConnectionFactory(string? connectionString = null)
            : base(connectionString)
        {
            global::Dapper.SqlMapper.AddTypeHandler(new LocalTimeTypeHandler());
            global::Dapper.SqlMapper.AddTypeHandler(new LocalDateTypeHandler());
            global::Dapper.SqlMapper.AddTypeHandler(new InstantTypeHandler());
        }

        public override async Task<DbConnection> OpenNewConnectionAsync()
        {
            var conn = new SqliteConnection(ConnectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            return conn;
        }
    }
}
