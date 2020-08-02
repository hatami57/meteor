using System.Data;
using Dapper;
using Meteor.Utils;
using NodaTime;

namespace Meteor.Database.Sqlite.TypeHandlers
{
    public class LocalDateTypeHandler : SqlMapper.TypeHandler<LocalDate>
    {
        public override LocalDate Parse(object value)
        {
            ((string) value).ToLocalDate().TryGetValue(default, out var result);
            return result;
        }

        public override void SetValue(IDbDataParameter parameter, LocalDate value)
        {
            parameter.Value = value.ToDateString();
        }
    }
}