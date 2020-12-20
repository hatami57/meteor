using System.Data;
using Dapper;
using Meteor.Utils;
using Microsoft.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;

namespace Meteor.Database.Dapper.MsSql.TypeHandlers
{
    public class MsSqlJsonTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            if (parameter is SqlParameter param)
            {
                //param.DbType = SqlDbType.NVarChar;
                param.Value = value?.ToJson();
            }
        }

        public override T Parse(object value)
        {
            return ((string)value).FromJson<T>();
        }
    }
}