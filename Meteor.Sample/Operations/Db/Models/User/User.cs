using Meteor.Operation.Db.Default;

namespace Meteor.Sample.Operations.Db.Models.User
{
    public record User : IDbModel<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}
