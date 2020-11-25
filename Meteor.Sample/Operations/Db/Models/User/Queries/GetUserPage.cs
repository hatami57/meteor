using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db;
using Meteor.Operation.Db.Default;

namespace Meteor.Sample.Operations.Db.Models.User.Queries
{
    public class GetUserPage : DbDefaultQueryPageAsync<User>
    {
        public GetUserPage(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory, "user")
        {
        }
    }
}