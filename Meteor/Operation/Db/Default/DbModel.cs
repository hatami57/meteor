namespace Meteor.Operation.Db.Default
{
    public class DbModel<TId> : IDbModel<TId>
    {
        public TId Id { get; set; }
    }
    
    public class DbModel : DbModel<int>
    {
    }
}
