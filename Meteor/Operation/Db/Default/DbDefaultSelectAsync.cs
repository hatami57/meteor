using System.Threading.Tasks;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultSelectAsync<T> : DbOperationAsync<T>
    {
        protected string TableName { get; set; }

        public DbDefaultSelectAsync(SharedLazyDbConnection sharedLazyDbConnection, string tableName)
            : base(sharedLazyDbConnection)
        {
            TableName = tableName;
        }

        protected override async Task ExecutionAsync() =>
            Result = await NewSql().SelectThisIdAsync<T>(TableName).ConfigureAwait(false);
    }
}