using System.Threading.Tasks;

namespace Meteor.Database.Dapper.Operations.Db.Default
{
    public class DbDefaultInsertAsync<TInput, TOutput> : DbOperationAsync<TInput, TOutput>
    {
        protected string TableName { get; }
        protected string ColumnNames { get; }
        protected string ColumnValues { get; }

        public DbDefaultInsertAsync(string tableName, string columnNames, string columnValues)
        {
            TableName = tableName;
            ColumnNames = columnNames;
            ColumnValues = columnValues;
        }

        protected override async Task ExecutionAsync() =>
            Output = await NewSql(sql => sql.InsertReturnId(TableName, ColumnNames, ColumnValues))
                .ExecuteScalarAsync<TOutput>().ConfigureAwait(false);
    }
}