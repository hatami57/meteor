using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Operation;
using Meteor.Operation.Db;
using Meteor.Sample.Operations.Db.Models.User.Dto;
using Meteor.Sample.Operations.Logging;
using Meteor.Sample.Operations.Logging.Types;

namespace Meteor.Sample.Operations.Db.Models.User.Commands
{
    public class AddUser : DbOperationAsync<AddUserInDto, int>, ILogInsert
    {
        private readonly OperationFactory _operationFactory;
        
        public LogDetails LogDetails { get; private set; }

        public AddUser(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory, OperationFactory operationFactory) : base(lazyDbConnection, sqlFactory)
        {
            _operationFactory = operationFactory;
        }

        protected override async Task ExecutionAsync()
        {
            Output = await NewSql(sql => sql.InsertReturnId("user",
                    "first_name, last_name, username",
                    "@FirstName, @LastName, @Username"))
                .ExecuteScalarAsync<int>();

            LogDetails = new LogDetails
            {
                UserId = 5,
                Result = Output > 0,
                Input = Input,
                Output = Output
            };
        }
    }
}
