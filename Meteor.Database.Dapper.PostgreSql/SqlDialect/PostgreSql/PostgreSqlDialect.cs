using Meteor.Database.Sql.SqlDialect;

namespace Meteor.Database.Dapper.PostgreSql.SqlDialect.PostgreSql
{
    public class PostgreSqlDialect : Sql.SqlDialect.SqlDialect
    {
        public override ISqlDialect InsertReturnId(string tableName, string? columnNames, string values, string idColumnName = "id") =>
            Insert(tableName, columnNames, values).AppendSql($"RETURNING {idColumnName};");
    }
}