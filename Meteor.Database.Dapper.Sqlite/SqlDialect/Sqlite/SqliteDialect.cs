using Meteor.Database.Dapper.SqlDialect;

namespace Meteor.Database.Dapper.Sqlite.SqlDialect.Sqlite
{
    public class SqliteDialect : Dapper.SqlDialect.SqlDialect
    {
        public override ISqlDialect InsertReturnId(string tableName, string? columnNames, string values, string idColumnName = "id") =>
            Insert(tableName, columnNames, values).EndStatement().AppendSql("SELECT last_insert_rowid();");

        public override ISqlDialect Offset(string? offset, string? limit)
        {
            limit = string.IsNullOrWhiteSpace(limit) ? "" : $"LIMIT {limit}";
            offset = string.IsNullOrWhiteSpace(offset) ? "" : $"OFFSET {offset}";
            
            return AppendSql($"{limit} {offset}".Trim());
        }
    }
}