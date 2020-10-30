using System.Threading.Tasks;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultQueryPageAsync<T> : DbQueryPageAsync<T>
    {
        protected string TableName { get; set; }
        
        public DbDefaultQueryPageAsync(SharedLazyDbConnection sharedLazyDbConnection, string tableName)
            : base(sharedLazyDbConnection)
        {
            TableName = tableName;
        }
        protected override async Task ExecutionAsync() => 
            Result = await NewSql().SelectQueryPageAsync(TableName, this).ConfigureAwait(false);
    }
}