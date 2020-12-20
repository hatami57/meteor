using System.Data;
using Dapper;
using Meteor.Utils;
using NodaTime;

namespace Meteor.Database.Dapper.Sqlite.TypeHandlers
{
    public class InstantTypeHandler : SqlMapper.TypeHandler<Instant>
    {
        public override Instant Parse(object value)
        {
            ((string) value).ToInstant().TryGetValue(default, out var result);
            return result;
        }

        public override void SetValue(IDbDataParameter parameter, Instant value)
        {
            parameter.Value = value.ToInstantString();
        }
    }
}