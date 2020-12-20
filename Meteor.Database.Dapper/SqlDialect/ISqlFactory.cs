namespace Meteor.Database.Dapper.SqlDialect
{
    public interface ISqlFactory
    {
        public ISqlDialect Create();
        public ISqlDialect Create<T>() where T : ISqlDialect, new();
    }
}