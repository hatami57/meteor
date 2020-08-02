using System.Data.Common;
using System.Threading.Tasks;
using Meteor.Database.PostgreSql.TypeHandlers;
using Npgsql;

namespace Meteor.Database.PostgreSql
{
    public class NpgsqlDbConnectionFactory : DbConnectionFactory
    {
        public NpgsqlDbConnectionFactory(string connectionString = null)
            : base(connectionString)
        {
            Dapper.SqlMapper.AddTypeHandler(new NpgsqlLocalDateTypeHandler());
            Dapper.SqlMapper.AddTypeHandler(new NpgsqlInstantTypeHandler());
            NpgsqlConnection.GlobalTypeMapper.UseNodaTime();
        }

        public override async Task<DbConnection> OpenNewConnectionAsync()
        {
            var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            return conn;
        }
    }
}
