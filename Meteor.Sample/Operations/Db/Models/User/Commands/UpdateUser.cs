using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db;
using Meteor.Sample.Operations.Db.Models.User.Dto;

namespace Meteor.Sample.Operations.Db.Models.User.Commands
{
    public class UpdateUser : DbOperationAsync<UpdateUserInDto, bool>
    {
        public UpdateUser(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory)
        {
        }

        protected override async Task ExecutionAsync()
        {
            Output = await NewSql(sql => sql.UpdateThisId("user", "first_name=@FirstName, last_name=@LastName, username=@Username"))
                .ExecuteAsync() > 0;
        }
    }
}
