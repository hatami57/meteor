using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db.Default;

namespace Meteor.Sample.Operations.Db.Models.User.Queries
{
    public class GetUser : DbDefaultSelectAsync<int, User>
    {
        public GetUser(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory, "user")
        {
        }
    }
}