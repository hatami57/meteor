namespace Meteor.Database.SqlDialect
{
    public class SqlFactory<T> : ISqlFactory where T : ISqlDialect, new()
    {
        public ISqlDialect Create() =>
            new T();

        public ISqlDialect Create<TDialect>() where TDialect : ISqlDialect, new() =>
            new TDialect();
    }
}