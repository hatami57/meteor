using System.ComponentModel;
using System.Threading.Tasks;
using Meteor.Database;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultInsertAsync<T> : DbOperationAsync<T>
    {
        protected string TableName { get; set; }
        protected string FieldNames { get; set; }
        protected string FieldValues { get; set; }

        public DbDefaultInsertAsync(SharedLazyDbConnection sharedLazyDbConnection, string tableName, string fieldNames, string fieldValues)
            : base(sharedLazyDbConnection)
        {
            TableName = tableName;
            FieldNames = fieldNames;
            FieldValues = fieldValues;
        }

        protected override async Task ExecutionAsync() =>
            Result = await NewSql().InsertGetIdPgSqlAsync<T>(TableName, FieldNames, FieldValues).ConfigureAwait(false);
    }
}