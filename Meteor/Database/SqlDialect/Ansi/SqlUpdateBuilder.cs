using System.Collections.Generic;

namespace Meteor.Database.SqlDialect.Ansi
{
    public class SqlUpdateBuilder : ISqlDialectUpdateBuilder
    {
        public string SqlText => _updateColumns.Count == 0
            ? ""
            : string.Join(",", _updateColumns);

        private readonly List<string> _updateColumns = new List<string>();

        public ISqlDialectUpdateBuilder Set(string setColumn)
        {
            _updateColumns.Add(setColumn);
            return this;
        }

        public ISqlDialectUpdateBuilder When(bool when, string setColumn) =>
            when ? Set(setColumn) : this;
    }
}