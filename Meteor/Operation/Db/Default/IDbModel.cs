namespace Meteor.Operation.Db.Default
{
    public interface IDbModel<TId>
    {
        public TId Id { get; }
    }
}
