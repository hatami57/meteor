using Meteor.Database.SqlDialect.Ansi;

namespace Meteor.Database.SqlDialect.MsSql
{
    public class MsSqlDialect : Sql
    {
        public new ISqlDialect InsertReturnId(string tableName, string columnNames, string values) =>
            AppendSql($"INSERT INTO {tableName} ({columnNames}) OUTPUT INSERTED.Id VALUES({values})");
    }
}