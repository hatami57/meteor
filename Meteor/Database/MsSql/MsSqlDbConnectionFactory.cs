using System.Data.Common;
using System.Threading.Tasks;
using Meteor.Database.MsSql.TypeHandlers;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace Meteor.Database.MsSql
{
    public class MsSqlDbConnectionFactory : DbConnectionFactory
    {
        public MsSqlDbConnectionFactory(string? connectionString = null)
            : base(connectionString)
        {
            Dapper.SqlMapper.AddTypeHandler(new MsSqlInstantTypeHandler());
            Dapper.SqlMapper.AddTypeHandler(new MsSqlLocalDateTypeHandler());
            Dapper.SqlMapper.AddTypeHandler(new MsSqlLocalTimeTypeHandler());
        }

        public override async Task<DbConnection> OpenNewConnectionAsync()
        {
            var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            return conn;
        }
    }
}
