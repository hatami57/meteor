using System.Threading.Tasks;

namespace Meteor.Database.Dapper.Operations.Db.Default
{
    public class DbDefaultSelectAsync<TId, TOutput> : DbOperationAsync<DbModel<TId>, TOutput> where TOutput : IDbModel<TId>
    {
        protected string TableName { get; set; }

        public DbDefaultSelectAsync(string tableName) => TableName = tableName;

        protected override async Task ExecutionAsync() =>
            Output = await NewSql(sql => sql.SelectThisId(TableName))
            .QueryFirstOrDefaultAsync<TOutput>().ConfigureAwait(false);
    }
}