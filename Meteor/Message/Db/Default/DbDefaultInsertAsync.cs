using System.ComponentModel;
using System.Threading.Tasks;
using Meteor.Database;

namespace Meteor.Message.Db.Default
{
    public class DbDefaultInsertAsync<T> : DbMessageAsync<T>
    {
        protected string TableName { get; set; }
        protected string FieldNames { get; set; }
        protected string FieldValues { get; set; }
        protected DatabaseType DatabaseType { get; set; }

        public DbDefaultInsertAsync(string tableName, string fieldNames, string fieldValues,
            DatabaseType databaseType = DatabaseType.PostgreSql)
        {
            TableName = tableName;
            FieldNames = fieldNames;
            FieldValues = fieldValues;
            DatabaseType = databaseType;
        }

        protected override Task<T> ExecuteMessageAsync() =>
            DatabaseType switch
            {
                DatabaseType.PostgreSql => NewSql().InsertGetIdPgSqlAsync<T>(TableName, FieldNames, FieldValues),
                DatabaseType.Sqlite => NewSql().InsertGetIdSqliteAsync<T>(TableName, FieldNames, FieldValues),
                _ => throw new InvalidEnumArgumentException(nameof(DatabaseType))
            };
    }
}