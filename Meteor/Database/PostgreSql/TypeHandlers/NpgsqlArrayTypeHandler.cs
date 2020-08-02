using System.Data;
using Dapper;
using Npgsql;

namespace Meteor.Database.PostgreSql.TypeHandlers
{
    public class NpgsqlArrayTypeHandler<T> : SqlMapper.TypeHandler<T[]>
    {
        public override void SetValue(IDbDataParameter parameter, T[] value)
        {
            if (parameter is NpgsqlParameter param)
                param.NpgsqlValue = value;
        }

        public override T[] Parse(object value)
        {
            return value as T[];
        }
    }
}