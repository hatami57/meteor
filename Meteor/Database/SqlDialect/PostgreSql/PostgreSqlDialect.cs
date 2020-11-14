namespace Meteor.Database.SqlDialect.PostgreSql
{
    public class PostgreSqlDialect : SqlDialect
    {
        public new ISqlDialect InsertReturnId(string tableName, string? columnNames, string values, string idColumnName = "id") =>
            Insert(tableName, columnNames, values).AppendSql($"RETURNING {idColumnName};");
    }
}