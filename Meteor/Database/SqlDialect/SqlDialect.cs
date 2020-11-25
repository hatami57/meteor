using System;
using System.Text;
using Meteor.Utils;

namespace Meteor.Database.SqlDialect
{
    public class SqlDialect : ISqlDialect
    {
        private readonly StringBuilder _sb;
        private char _lastChar;
        
        public string SqlText
        {
            get => _sb.ToString();
            set
            {
                _sb.Clear();
                _sb.Append(value);
                _lastChar = value?.Length > 0 ? value[^1] : char.MinValue;
            }
        }

        public SqlDialect(string? sqlText = null)
        {
            _sb = new StringBuilder(sqlText);
            _lastChar = char.MinValue;
        }

        public virtual ISqlDialect Select(string tableName, string columnNames = "*") =>
            AppendSql($"SELECT {columnNames} FROM {tableName}");

        public virtual ISqlDialect InnerJoin(string tableName, string onClause) =>
            Join(tableName, onClause);

        public virtual ISqlDialect LeftJoin(string tableName, string onClause) =>
            Join(tableName, onClause, "LEFT");

        public virtual ISqlDialect RightJoin(string tableName, string onClause) =>
            Join(tableName, onClause, "RIGHT");

        public virtual ISqlDialect FullJoin(string tableName, string onClause) =>
            Join(tableName, onClause, "FULL");

        private ISqlDialect Join(string tableName, string onClause, string type = "INNER") =>
            AppendSql($" {type} JOIN {tableName} ON {onClause}");

        public virtual ISqlDialect Where(string where) =>
            AppendSql("WHERE " + where);

        public virtual ISqlDialect Where(Action<SqlWhereBuilder> whereBuilder)
        {
            if (whereBuilder == null) throw new ArgumentNullException(nameof(whereBuilder));
            
            var builder = new SqlWhereBuilder();
            
            whereBuilder(builder);
            return AppendSql("WHERE " + builder.SqlText);
        }

        public virtual ISqlDialect GroupBy(string columnNames) =>
            AppendSql("GROUP BY " + columnNames);

        public virtual ISqlDialect Having(string having) =>
            AppendSql("HAVING " + having);

        public virtual ISqlDialect OrderBy(string columnNames) =>
            AppendSql("ORDER BY " + columnNames);

        public virtual ISqlDialect Offset(string? offset, string? fetchFirst)
        {
            offset = string.IsNullOrWhiteSpace(offset) ? "" : $"OFFSET {offset}";
            fetchFirst = string.IsNullOrWhiteSpace(fetchFirst) ? "" : $"FETCH FIRST {fetchFirst} ROWS ONLY";
            return AppendSql($"{offset} {fetchFirst}".Trim());
        }

        public virtual ISqlDialect Insert(string tableName, string? columnNames, string values) =>
            InsertCustomValues(tableName, columnNames, $"VALUES ({values})");

        public virtual ISqlDialect InsertCustomValues(string tableName, string? columnNames, string customValues)
        {
            columnNames = string.IsNullOrWhiteSpace(columnNames) ? "" : $" ({columnNames})";
            return AppendSql($"INSERT INTO {tableName}{columnNames} {customValues}");
        }

        public virtual ISqlDialect InsertReturnId(string tableName, string columnNames, string values, string idColumnName = "id") =>
            throw Errors.InvalidOperation("not_implemented");

        public virtual ISqlDialect Update(string tableName, string setColumns) =>
            AppendSql($"UPDATE {tableName} SET {setColumns}");

        public virtual ISqlDialect Update(string tableName, Action<SqlUpdateBuilder> updateBuilder)
        {
            if (updateBuilder == null) throw new ArgumentNullException(nameof(updateBuilder));
            
            var builder = new SqlUpdateBuilder();
            updateBuilder(builder);
            return AppendSql($"UPDATE {tableName} SET {builder.SqlText}");
        }

        public virtual ISqlDialect Delete(string tableName) =>
            AppendSql($"DELETE FROM {tableName}");

        public virtual ISqlDialect EndStatement()
        {
            _sb.Append(';');
            return this;
        }

        public ISqlDialect AppendSql(ReadOnlySpan<char> sql)
        {
            if (sql.IsWhiteSpace())
                return this;
            
            var needWhitespace = _sb.Length > 0 && !char.IsWhiteSpace(_lastChar);
            if (needWhitespace)
                _sb.Append(' ');
            
            _sb.Append(sql);
            _lastChar = sql[^1];

            return this;
        }

        public ISqlDialect Clear()
        {
            _sb.Clear();
            _lastChar = char.MinValue;
            return this;
        }
    }
}