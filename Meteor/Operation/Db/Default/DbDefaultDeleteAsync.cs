using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultDeleteAsync : DbOperationAsync<bool>
    {
        protected string TableName { get; set; }

        public DbDefaultDeleteAsync(LazyDbConnection lazyDbConnection, ISqlDialect? sqlDialect, string tableName)
            : base(lazyDbConnection, sqlDialect)
        {
            TableName = tableName;
        }

        protected override async Task ExecutionAsync() =>
            Result = await NewSql(sql => sql.DeleteThisId(TableName)).ExecuteAsync()
                .ConfigureAwait(false) > 0;
    }
}