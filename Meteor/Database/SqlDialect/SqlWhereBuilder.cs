using System.Collections.Generic;

namespace Meteor.Database.SqlDialect
{
    public class SqlWhereBuilder
    {
        public string SqlText => _whereConditions.Count == 0
            ? ""
            : string.Join(" AND ", _whereConditions);

        private readonly List<string> _whereConditions = new List<string>();

        public SqlWhereBuilder Where(string condition)
        {
            _whereConditions.Add(condition);
            return this;
        }

        public SqlWhereBuilder And(string condition) =>
            Where(condition);

        public SqlWhereBuilder When(bool when, string condition) =>
            when ? Where(condition) : this;
    }
}