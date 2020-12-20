using Meteor.Operation;

namespace Meteor.Database.Dapper.Operations.Db
{
    public class DbOperationAttribute : OperationAttribute
    {
        public DbOperationAttribute(object? input = null, object? output = null)
            : base(input, output)
        {
            Type = typeof(DbOperationAsync<,>);
        }
    }
}