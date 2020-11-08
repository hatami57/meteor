namespace Meteor.Database.SqlDialect.Sqlite
{
    public class SqliteDialect : SqlDialect
    {
        public new ISqlDialect InsertReturnId(string tableName, string columnNames, string values) =>
            Insert(tableName, columnNames, values).EndStatement().AppendSql("SELECT last_insert_rowid();");
    }
}