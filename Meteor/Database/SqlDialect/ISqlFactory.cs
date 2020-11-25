namespace Meteor.Database.SqlDialect
{
    public interface ISqlFactory
    {
        public ISqlDialect Create();
        public ISqlDialect Create<T>() where T : ISqlDialect, new();
    }
}