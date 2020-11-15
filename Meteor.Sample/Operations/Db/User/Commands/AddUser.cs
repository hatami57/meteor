using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db;
using Meteor.Sample.Operations.Db.User.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meteor.Sample.Operations.Db.User.Commands
{
    public class AddUser : DbOperationAsync<AddUserInDto, int>
    {
        public AddUser(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory)
        {

        }

        protected override async Task ExecutionAsync()
        {
            Output = await NewSql(sql => sql.InsertReturnId("user", "first_name, last_name, username", "@FirstName, @LastName, @Username"))
                .ExecuteScalarAsync<int>();
        }
    }
}
