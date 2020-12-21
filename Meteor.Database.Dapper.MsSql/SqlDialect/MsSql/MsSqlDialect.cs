using Meteor.Database.Sql.SqlDialect;

namespace Meteor.Database.Dapper.MsSql.SqlDialect.MsSql
{
    public class MsSqlDialect : Sql.SqlDialect.SqlDialect
    {
        public override ISqlDialect InsertReturnId(string tableName, string? columnNames, string values, string idColumnName = "Id") =>
            InsertCustomValues(tableName, columnNames, $"OUTPUT INSERTED.{idColumnName} VALUES ({values})");
    }
}