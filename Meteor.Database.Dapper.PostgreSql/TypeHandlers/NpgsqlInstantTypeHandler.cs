using System.Data;
using Dapper;
using NodaTime;

namespace Meteor.Database.Dapper.PostgreSql.TypeHandlers
{
    public class NpgsqlInstantTypeHandler : SqlMapper.TypeHandler<Instant>
    {
        public override Instant Parse(object value)
        {
            return (Instant) value;
        }

        public override void SetValue(IDbDataParameter parameter, Instant value)
        {
            parameter.Value = value;
        }
    }
}