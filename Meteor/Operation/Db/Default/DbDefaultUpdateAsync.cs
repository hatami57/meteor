using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;

namespace Meteor.Operation.Db.Default
{
    public class DbDefaultUpdateAsync : DbOperationAsync<bool>
    {
        protected string TableName { get; set; }
        protected string SetColumns { get; set; }

        public DbDefaultUpdateAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory, string tableName,
            string setColumns)
            : base(lazyDbConnection, sqlFactory)
        {
            TableName = tableName;
            SetColumns = setColumns;
        }

        protected override async Task ExecutionAsync() =>
            Result = await NewSql(sql => sql.UpdateThisId(TableName, SetColumns)).ExecuteAsync()
                .ConfigureAwait(false) > 0;
    }
}