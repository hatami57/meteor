using System.Collections.Generic;

namespace Meteor.Database.SqlDialect.Ansi
{
    public class SqlWhereBuilder : ISqlDialectWhereBuilder
    {
        public string SqlText => _whereConditions.Count == 0
            ? ""
            : string.Join(" AND ", _whereConditions);

        private readonly List<string> _whereConditions = new List<string>();

        public ISqlDialectWhereBuilder Where(string condition)
        {
            _whereConditions.Add(condition);
            return this;
        }

        public ISqlDialectWhereBuilder And(string condition) =>
            Where(condition);

        public ISqlDialectWhereBuilder When(bool when, string condition) =>
            when ? Where(condition) : this;
    }
}