using System.Data.Common;
using System.Threading.Tasks;
using Meteor.Database.Dapper.MsSql.TypeHandlers;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace Meteor.Database.Dapper.MsSql
{
    public class MsSqlDbConnectionFactory : DbConnectionFactory
    {
        public MsSqlDbConnectionFactory(string? connectionString = null)
            : base(connectionString)
        {
            global::Dapper.SqlMapper.AddTypeHandler(new MsSqlInstantTypeHandler());
            global::Dapper.SqlMapper.AddTypeHandler(new MsSqlLocalDateTypeHandler());
            global::Dapper.SqlMapper.AddTypeHandler(new MsSqlLocalTimeTypeHandler());
        }

        public override async Task<DbConnection> OpenNewConnectionAsync()
        {
            var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            return conn;
        }
    }
}
