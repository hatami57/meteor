using System.Data;
using Dapper;
using NodaTime;

namespace Meteor.Database.Dapper.PostgreSql.TypeHandlers
{
    public class NpgsqlLocalTimeTypeHandler : SqlMapper.TypeHandler<LocalTime>
    {
        public override LocalTime Parse(object value)
        {
            return (LocalTime) value;
        }

        public override void SetValue(IDbDataParameter parameter, LocalTime value)
        {
            parameter.Value = value;
        }
    }
}