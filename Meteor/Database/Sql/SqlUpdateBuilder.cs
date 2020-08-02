﻿using System.Collections.Generic;

 namespace Meteor.Database.Sql
{
    public class SqlUpdateBuilder
    {
        private readonly string _tableName;
        private readonly List<string> _updateFields = new List<string>();

        public SqlUpdateBuilder(string tableName, string setFields = null)
        {
            _tableName = tableName;
            _updateFields.Add(setFields);
        }
        
        public SqlUpdateBuilder Set(string updateField)
        {
            _updateFields.Add(updateField);
            return this;
        }

        public SqlUpdateBuilder When(bool when, string updateField) =>
            when ? Set(updateField) : this;

        public string GetSql()
        {
            return _updateFields.Count == 0
                ? ""
                : $"UPDATE {_tableName} SET " + string.Join(",", _updateFields);
        }
    }
}
