using System.Data.Common;
using System.Threading.Tasks;
using Meteor.Database.Dapper.PostgreSql.TypeHandlers;
using Npgsql;

namespace Meteor.Database.Dapper.PostgreSql
{
    public class NpgsqlDbConnectionFactory : DbConnectionFactory
    {
        public NpgsqlDbConnectionFactory(string? connectionString = null)
            : base(connectionString)
        {
            global::Dapper.SqlMapper.AddTypeHandler(new NpgsqlLocalTimeTypeHandler());
            global::Dapper.SqlMapper.AddTypeHandler(new NpgsqlLocalDateTypeHandler());
            global::Dapper.SqlMapper.AddTypeHandler(new NpgsqlInstantTypeHandler());
            global::Dapper.SqlMapper.AddTypeHandler(new NpgsqlGeometryPointTypeHandler());
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
