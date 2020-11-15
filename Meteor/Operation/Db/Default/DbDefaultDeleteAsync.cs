using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultDeleteAsync<TId> : DbOperationAsync<DefaultId<TId>, bool>
    {
        protected string TableName { get; set; }

        public DbDefaultDeleteAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory, string tableName)
            : base(lazyDbConnection, sqlFactory)
        {
            TableName = tableName;
        }

        protected override async Task ExecutionAsync() =>
            Output = await NewSql(sql => sql.DeleteThisId(TableName)).ExecuteAsync()
                .ConfigureAwait(false) > 0;
    }
}