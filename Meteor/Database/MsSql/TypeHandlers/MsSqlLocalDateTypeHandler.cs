using System;
using System.Data;
using Dapper;
using NodaTime;

namespace Meteor.Database.MsSql.TypeHandlers
{
    public class MsSqlLocalDateTypeHandler : SqlMapper.TypeHandler<LocalDate>
    {
        public override LocalDate Parse(object value)
        {
            return LocalDate.FromDateTime((DateTime) value);
        }

        public override void SetValue(IDbDataParameter parameter, LocalDate value)
        {
            parameter.Value = value.ToDateTimeUnspecified();
        }
    }
}