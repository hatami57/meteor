using System;
using System.Data;
using Dapper;
using NodaTime;

namespace Meteor.Database.Dapper.MsSql.TypeHandlers
{
    public class MsSqlLocalTimeTypeHandler : SqlMapper.TypeHandler<LocalTime>
    {
        public override LocalTime Parse(object value)
        {
            return LocalTime.FromTicksSinceMidnight(((TimeSpan)value).Ticks);
        }

        public override void SetValue(IDbDataParameter parameter, LocalTime value)
        {
            parameter.Value = value.TickOfDay;
        }
    }
}