using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
    public abstract class DbQueryPageAsync<T> : DbOperationAsync<QueryPage<T>>
    {
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 10;
        public int Skip => (Page - 1) * Take;

        protected DbQueryPageAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory)
            : base(lazyDbConnection, sqlFactory)
        {
        }

        protected override Task ValidatePropertiesAsync()
        {
            if (Page <= 0 || Take <= 0)
                throw Errors.InvalidInput();

            return Task.CompletedTask;
        }
    }
}