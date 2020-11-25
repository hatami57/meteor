using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db;
using Serilog;

namespace Meteor.Sample.Operations.Db
{
    public class CreateDatabase : DbOperationAsync
    {
        public CreateDatabase(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory)
        {
        }

        protected override Task ExecutionAsync()
        {
            const string sqlText = @"
CREATE TABLE IF NOT EXISTS user (
    id integer NOT NULL PRIMARY KEY,
    first_name text NULL,
    last_name text NULL,
    username text NULL
);
";
            return NewSql(sqlText).ExecuteAsync();
        }

        protected override Task OnSuccessAsync()
        {
            Log.Information("Database created");
            return Task.CompletedTask;
        }
    }
}