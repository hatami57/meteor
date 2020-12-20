namespace Meteor.Database.Dapper.Operations.Db.Default
{
    public record DbModel<TId> : IDbModel<TId>
    {
        public TId Id { get; set; }
    }
    
    public record DbModel : DbModel<int>
    {
    }
}
