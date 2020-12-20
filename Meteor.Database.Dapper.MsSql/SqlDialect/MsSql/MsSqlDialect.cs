using Meteor.Database.Dapper.SqlDialect;

namespace Meteor.Database.Dapper.MsSql.SqlDialect.MsSql
{
    public class MsSqlDialect : Dapper.SqlDialect.SqlDialect
    {
        public override ISqlDialect InsertReturnId(string tableName, string? columnNames, string values, string idColumnName = "Id") =>
            InsertCustomValues(tableName, columnNames, $"OUTPUT INSERTED.{idColumnName} VALUES ({values})");
    }
}