using System;
using System.Data;
using Dapper;
using Meteor.Utils;
using NodaTime;

namespace Meteor.Database.Dapper.MsSql.TypeHandlers
{
    public class MsSqlInstantTypeHandler : SqlMapper.TypeHandler<Instant>
    {
        public override Instant Parse(object value)
        {
            return DateTime.SpecifyKind((DateTime) value, DateTimeKind.Utc).ToInstant();
        }

        public override void SetValue(IDbDataParameter parameter, Instant value)
        {
            parameter.Value = value.ToDateTimeUtc();
        }
    }
}