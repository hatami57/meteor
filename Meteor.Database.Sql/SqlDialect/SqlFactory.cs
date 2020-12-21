namespace Meteor.Database.Sql.SqlDialect
{
    public class SqlFactory<T> : ISqlFactory where T : ISqlDialect, new()
    {
        public ISqlDialect Create() => Create<T>();

        public ISqlDialect Create<TDialect>() where TDialect : ISqlDialect, new() =>
            new TDialect {SqlFactory = this};
    }
}