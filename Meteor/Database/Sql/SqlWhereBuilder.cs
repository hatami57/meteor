﻿using System.Collections.Generic;

 namespace Meteor.Database.Sql
{
    public class SqlWhereBuilder
    {
        private readonly List<string> _whereConditions = new List<string>();

        public SqlWhereBuilder(string where = null)
        {
            if (!string.IsNullOrWhiteSpace(where))
                _whereConditions.Add(where);
        }
        
        public SqlWhereBuilder Where(string condition)
        {
            _whereConditions.Add(condition);
            return this;
        }

        public SqlWhereBuilder And(string condition) =>
            Where(condition);

        public SqlWhereBuilder When(bool when, string condition) =>
            when ? Where(condition) : this;

        public string GetSql()
        {
            return _whereConditions.Count == 0
                ? ""
                : "WHERE " + string.Join(" AND ", _whereConditions);
        }
    }
}
