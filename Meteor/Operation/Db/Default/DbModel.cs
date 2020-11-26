namespace Meteor.Operation.Db.Default
{
    public record DbModel<TId> : IDbModel<TId>
    {
        public TId Id { get; set; }
    }
    
    public record DbModel : DbModel<int>
    {
    }
}
