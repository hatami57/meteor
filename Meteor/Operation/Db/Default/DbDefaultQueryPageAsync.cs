using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultQueryPageAsync<T> : DbQueryPageAsync<T>
    {
        protected string TableName { get; set; }
        
        public DbDefaultQueryPageAsync(LazyDbConnection lazyDbConnection, ISqlDialect? sqlDialect, string tableName)
            : base(lazyDbConnection, sqlDialect)
        {
            TableName = tableName;
        }
        protected override async Task ExecutionAsync() => 
            Result = await this.SelectQueryPageAsync(SqlDialect, TableName).ConfigureAwait(false);
    }
}