using System.Collections.Generic;
using System.Data;
using Dapper;
using Npgsql;

namespace Meteor.Database.PostgreSql.TypeHandlers
{
    public class NpgsqlListTypeHandler<T> : SqlMapper.TypeHandler<List<T>>
    {
        public override void SetValue(IDbDataParameter parameter, List<T> value)
        {
            if (parameter is NpgsqlParameter param)
                param.NpgsqlValue = value;
        }

        public override List<T> Parse(object value)
        {
            return value as List<T>;
        }
    }
}