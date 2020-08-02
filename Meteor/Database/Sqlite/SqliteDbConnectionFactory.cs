using System.Data.Common;
using System.Threading.Tasks;
using Meteor.Database.Sqlite.TypeHandlers;
using Microsoft.Data.Sqlite;

namespace Meteor.Database.Sqlite
{
    public class SqliteDbConnectionFactory : DbConnectionFactory
    {
        public SqliteDbConnectionFactory(string connectionString = null)
            : base(connectionString)
        {
            Dapper.SqlMapper.AddTypeHandler(new LocalDateTypeHandler());
            Dapper.SqlMapper.AddTypeHandler(new InstantTypeHandler());
        }

        public override async Task<DbConnection> OpenNewConnectionAsync()
        {
            var conn = new SqliteConnection(ConnectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            return conn;
        }
    }
}
