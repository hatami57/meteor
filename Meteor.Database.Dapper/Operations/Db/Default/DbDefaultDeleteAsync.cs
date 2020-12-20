using System.Threading.Tasks;

namespace Meteor.Database.Dapper.Operations.Db.Default
{
    public class DbDefaultDeleteAsync<TId> : DbOperationAsync<DbModel<TId>, bool>
    {
        protected string TableName { get; set; }

        public DbDefaultDeleteAsync(string tableName)
        {
            TableName = tableName;
        }

        protected override async Task ExecutionAsync() =>
            Output = await NewSql(sql => sql.DeleteThisId(TableName)).ExecuteAsync()
                .ConfigureAwait(false) > 0;
    }
}