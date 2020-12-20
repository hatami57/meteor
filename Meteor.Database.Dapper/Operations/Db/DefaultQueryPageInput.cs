namespace Meteor.Database.Dapper.Operations.Db
{
    public class DefaultQueryPageInput : IQueryPageInput
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public int Offset => (Page - 1) * Take;
    }
}
