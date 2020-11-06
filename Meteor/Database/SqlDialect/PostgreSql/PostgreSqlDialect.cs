using Meteor.Database.SqlDialect.Ansi;

namespace Meteor.Database.SqlDialect.PostgreSql
{
    public class PostgreSqlDialect : Sql
    {
        public new ISqlDialect InsertReturnId(string tableName, string columnNames, string values) =>
            Insert(tableName, columnNames, values).AppendSql("RETURNING id;");
    }
}