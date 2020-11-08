using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultSelectAsync<T> : DbOperationAsync<T>
    {
        protected string TableName { get; set; }

        public DbDefaultSelectAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory, string tableName)
            : base(lazyDbConnection, sqlFactory)
        {
            TableName = tableName;
        }

        protected override async Task ExecutionAsync() =>
            Result = await NewSql(sql => sql.SelectThisId(TableName)).QueryFirstOrDefaultAsync<T>()
                .ConfigureAwait(false);
    }
}