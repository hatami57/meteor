using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db.Default;

namespace Meteor.Sample.Operations.Db.Models.User.Commands
{
    public class DeleteUser : DbDefaultDeleteAsync<int>
    {
        public DeleteUser(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory, "user")
        {
        }
    }
}
