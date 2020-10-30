using System.Threading.Tasks;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultUpdateAsync : DbOperationAsync<bool>
    {
        protected string TableName { get; set; }
        protected string SetFields { get; set; }

        public DbDefaultUpdateAsync(SharedLazyDbConnection sharedLazyDbConnection, string tableName, string setFields)
            : base(sharedLazyDbConnection)
        {
            TableName = tableName;
            SetFields = setFields;
        }

        protected override async Task ExecutionAsync() =>
            Result = await NewSql().UpdateThisIdAsync(TableName, SetFields)
                .ConfigureAwait(false) > 0;
    }
}