namespace Meteor.Operation.Db
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