using System.Collections.Generic;

namespace Meteor.Database.Sql.SqlDialect
{
    public class SqlUpdateBuilder
    {
        public string SqlText => _updateColumns.Count == 0
            ? ""
            : string.Join(", ", _updateColumns);

        private readonly List<string> _updateColumns = new List<string>();

        public SqlUpdateBuilder Set(string setColumn)
        {
            _updateColumns.Add(setColumn);
            return this;
        }

        public SqlUpdateBuilder When(bool when, string setColumn) =>
            when ? Set(setColumn) : this;
    }
}