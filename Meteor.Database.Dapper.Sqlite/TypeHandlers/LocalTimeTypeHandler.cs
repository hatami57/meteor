using System.Data;
using Dapper;
using Meteor.Utils;
using NodaTime;

namespace Meteor.Database.Dapper.Sqlite.TypeHandlers
{
    public class LocalTimeTypeHandler : SqlMapper.TypeHandler<LocalTime>
    {
        public override LocalTime Parse(object value)
        {
            ((string) value).ToLocalTime().TryGetValue(default, out var result);
            return result;
        }

        public override void SetValue(IDbDataParameter parameter, LocalTime value)
        {
            parameter.Value = value.ToTimeString();
        }
    }
}