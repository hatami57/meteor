namespace Meteor.Database.SqlDialect
{
    public interface ISqlDialectUpdateBuilder
    {
        public string SqlText { get; }

        public ISqlDialectUpdateBuilder Set(string setColumn);
        public ISqlDialectUpdateBuilder When(bool when, string setColumn);
    }
}
