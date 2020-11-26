using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultQueryPageAsync<TOutput> : DbQueryPageAsync<DefaultQueryPageInput, TOutput>
    {
        protected string TableName { get; set; }

        public DbDefaultQueryPageAsync(string tableName) => TableName = tableName;

        protected override async Task ExecutionAsync() =>
            Output = await this.SelectQueryPageAsync(SqlFactory, TableName).ConfigureAwait(false);
    }
}