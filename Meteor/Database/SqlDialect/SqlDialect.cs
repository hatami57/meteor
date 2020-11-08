using System;
using Meteor.Utils;

namespace Meteor.Database.SqlDialect
{
    public class SqlDialect : ISqlDialect
    {
        public string SqlText { get; set; }

        public SqlDialect(string? sqlText = null)
        {
            SqlText = sqlText ?? "";
        }

        public ISqlDialect Select(string tableName, string columnNames = "*") =>
            AppendSql($"SELECT {columnNames} FROM {tableName}");

        public ISqlDialect InnerJoin(string tableName, string onClause) =>
            Join(tableName, onClause);

        public ISqlDialect LeftJoin(string tableName, string onClause) =>
            Join(tableName, onClause, "LEFT");

        public ISqlDialect RightJoin(string tableName, string onClause) =>
            Join(tableName, onClause, "RIGHT");

        public ISqlDialect FullJoin(string tableName, string onClause) =>
            Join(tableName, onClause, "FULL");

        private ISqlDialect Join(string tableName, string onClause, string type = "INNER") =>
            AppendSql($" {type} JOIN {tableName} ON {onClause}");

        public ISqlDialect Where(string where) =>
            AppendSql("WHERE " + where);

        public ISqlDialect Where(Action<SqlWhereBuilder> whereBuilder)
        {
            if (whereBuilder == null) throw new ArgumentNullException(nameof(whereBuilder));
            
            var builder = new SqlWhereBuilder();
            
            whereBuilder(builder);
            return AppendSql("WHERE " + builder.SqlText);
        }

        public ISqlDialect GroupBy(string columnNames) =>
            AppendSql("GROUP BY " + columnNames);

        public ISqlDialect Having(string having) =>
            AppendSql("HAVING " + having);

        public ISqlDialect OrderBy(string columnNames) =>
            AppendSql("ORDER BY " + columnNames);

        public ISqlDialect Offset(string? offset, string? fetchFirst)
        {
            offset = string.IsNullOrWhiteSpace(offset) ? "" : $"OFFSET {offset}";
            fetchFirst = string.IsNullOrWhiteSpace(fetchFirst) ? "" : $"FETCH FIRST {fetchFirst} ROWS ONLY";
            return AppendSql($"{offset} {fetchFirst}".Trim());
        }

        public ISqlDialect Insert(string tableName, string columnNames, string values) =>
            AppendSql($"INSERT INTO {tableName} ({columnNames}) VALUES ({values})");

        public ISqlDialect InsertReturnId(string tableName, string columnNames, string values) =>
            throw Errors.InvalidOperation("not_implemented");

        public ISqlDialect Update(string tableName, string setColumns) =>
            AppendSql($"UPDATE {tableName} SET {setColumns}");

        public ISqlDialect Update(string tableName, Action<SqlUpdateBuilder> updateBuilder)
        {
            if (updateBuilder == null) throw new ArgumentNullException(nameof(updateBuilder));
            
            var builder = new SqlUpdateBuilder();
            updateBuilder(builder);
            return AppendSql($"UPDATE {tableName} SET {builder.SqlText}");
        }

        public ISqlDialect Delete(string tableName) =>
            AppendSql($"DELETE FROM {tableName}");

        public ISqlDialect EndStatement()
        {
            SqlText += ";";
            return this;
        }

        public ISqlDialect AppendSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return this;

            SqlText += " " + sql.Trim();
            return this;
        }

        public ISqlDialect Clear()
        {
            SqlText = "";
            return this;
        }
    }
}