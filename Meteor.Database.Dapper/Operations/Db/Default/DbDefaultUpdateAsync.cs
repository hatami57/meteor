using System.Threading.Tasks;

namespace Meteor.Database.Dapper.Operations.Db.Default
{
    public class DbDefaultUpdateAsync<TId, TInput> : DbOperationAsync<TInput, bool> where TInput : IDbModel<TId>
    {
        protected string TableName { get; set; }
        protected string SetColumns { get; set; }

        public DbDefaultUpdateAsync(string tableName, string setColumns)
        {
            TableName = tableName;
            SetColumns = setColumns;
        }

        protected override async Task ExecutionAsync() =>
            Output = await NewSql(sql => sql.UpdateThisId(TableName, SetColumns))
            .ExecuteAsync().ConfigureAwait(false) > 0;
    }
}