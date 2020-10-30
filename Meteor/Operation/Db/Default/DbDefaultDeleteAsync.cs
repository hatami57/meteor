using System.Threading.Tasks;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultDeleteAsync : DbOperationAsync<bool>
    {
        protected string TableName { get; set; }

        public DbDefaultDeleteAsync(SharedLazyDbConnection sharedLazyDbConnection, string tableName)
            : base(sharedLazyDbConnection)
        {
            TableName = tableName;
        }

        protected override async Task ExecutionAsync() =>
            Result = await NewSql().DeleteThisIdAsync(TableName)
                .ConfigureAwait(false) > 0;
    }
}