using System;
using System.Collections.Generic;
using Meteor.Utils;

namespace Meteor.Database.SqlDialect
{
    public class SqlWithBuilder
    {
        public ISqlFactory SqlFactory { get; set; }
        public string SqlText => _withs.Count == 0
            ? ""
            : string.Join(", ", _withs);

        private readonly List<string> _withs = new List<string>();

        public SqlWithBuilder With(string name, string sql)
        {
            _withs.Add($"{name} AS ({sql})");
            return this;
        }

        public SqlWithBuilder With(string name, Func<ISqlDialect, ISqlDialect> sql)
        {
            if (sql == null) throw Errors.InvalidInput("sql==null");
            
            _withs.Add($"{name} AS ({sql(SqlFactory.Create()).SqlText})");
            return this;
        }
    }
}