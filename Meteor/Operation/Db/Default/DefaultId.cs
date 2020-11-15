namespace Meteor.Operation.Db.Default
{
    public class DefaultId : IHaveId
    {
        public int Id { get; set; }
    }

    public class DefaultId<T> : IHaveId<T>
    {
        public T Id { get; set; }
    }
}
