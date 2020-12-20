namespace Meteor.Database.Dapper.Operations.Db.Default
{
    public interface IDbModel<TId>
    {
        public TId Id { get; }
    }
}
