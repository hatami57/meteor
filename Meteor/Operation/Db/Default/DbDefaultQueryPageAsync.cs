using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultQueryPageAsync<T> : DbQueryPageAsync<T>
    {
        protected string TableName { get; set; }
        
        public DbDefaultQueryPageAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory, string tableName)
            : base(lazyDbConnection, sqlFactory)
        {
            TableName = tableName;
        }
        protected override async Task ExecutionAsync() => 
            Result = await this.SelectQueryPageAsync(SqlFactory, TableName).ConfigureAwait(false);
    }
}