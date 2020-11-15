namespace Meteor.Database.SqlDialect.MsSql
{
    public class MsSqlDialect : SqlDialect
    {
        public override ISqlDialect InsertReturnId(string tableName, string? columnNames, string values, string idColumnName = "Id") =>
            InsertCustomValues(tableName, columnNames, $"OUTPUT INSERTED.{idColumnName} VALUES ({values})");
    }
}