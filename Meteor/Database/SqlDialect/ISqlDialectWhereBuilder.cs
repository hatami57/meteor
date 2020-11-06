namespace Meteor.Database.SqlDialect
{
    public interface ISqlDialectWhereBuilder
    {
        public string SqlText { get; }
        
        public ISqlDialectWhereBuilder Where(string condition);
        public ISqlDialectWhereBuilder And(string condition);
        public ISqlDialectWhereBuilder When(bool when, string condition);
    }
}