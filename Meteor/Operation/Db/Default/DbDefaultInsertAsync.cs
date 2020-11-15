using System.ComponentModel;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultInsertAsync<TOutput> : DbOutOperationAsync<TOutput>
    {
        protected string TableName { get; set; }
        protected string ColumnNames { get; set; }
        protected string ColumnValues { get; set; }

        public DbDefaultInsertAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory, string tableName,
            string columnNames, string columnValues)
            : base(lazyDbConnection, sqlFactory)
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