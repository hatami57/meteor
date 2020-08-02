using System.Data;
using Dapper;
using NodaTime;

namespace Meteor.Database.PostgreSql.TypeHandlers
{
    public class NpgsqlLocalDateTypeHandler : SqlMapper.TypeHandler<LocalDate>
    {
        public override LocalDate Parse(object value)
        {
            return (LocalDate) value;
        }

        public override void SetValue(IDbDataParameter parameter, LocalDate value)
        {
            parameter.Value = value;
        }
    }
}