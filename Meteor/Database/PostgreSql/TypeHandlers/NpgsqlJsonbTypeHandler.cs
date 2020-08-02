using System.Data;
using Dapper;
using Meteor.Utils;
using Npgsql;
using NpgsqlTypes;

namespace Meteor.Database.PostgreSql.TypeHandlers
{
    public class NpgsqlJsonbTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            if (parameter is NpgsqlParameter param)
            {
                param.NpgsqlDbType = NpgsqlDbType.Jsonb;
                param.NpgsqlValue = value.ToJson();
            }
        }

        public override T Parse(object value)
        {
            return ((string)value).FromJson<T>();
        }
    }
}