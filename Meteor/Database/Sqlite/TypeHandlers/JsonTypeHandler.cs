using System.Data;
using Dapper;
using Meteor.Utils;

namespace Meteor.Database.Sqlite.TypeHandlers
{
    public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override T Parse(object value)
        {
            return ((string) value).FromJson<T>();
        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = value.ToJson();
        }
    }
}