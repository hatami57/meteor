using System;

namespace Meteor.Database.Sql.SqlDialect
{
    public interface ISqlDialect
    {
        public ISqlFactory SqlFactory { get; set; }
        public string SqlText { get; set; }

        public ISqlDialect With(string with);
        public ISqlDialect With(Action<SqlWithBuilder> withBuilder);
        public ISqlDialect Select(string tableName, string columnNames = "*");
        public ISqlDialect InnerJoin(string tableName, string onClause);
        public ISqlDialect LeftJoin(string tableName, string onClause);
        public ISqlDialect RightJoin(string tableName, string onClause);
        public ISqlDialect FullJoin(string tableName, string onClause);
        public ISqlDialect CrossJoin(string tableName);
        public ISqlDialect Where(string where);
        public ISqlDialect Where(Action<SqlWhereBuilder> whereBuilder);
        public ISqlDialect GroupBy(string columnNames);
        public ISqlDialect Having(string having);
        public ISqlDialect OrderBy(string columnNames);
        public ISqlDialect Offset(string? offset, string? fetchFirst);

        public ISqlDialect Insert(string tableName, string? columnNames, string values);
        public ISqlDialect InsertCustomValues(string tableName, string? columnNames, string customValues);
        public ISqlDialect InsertReturnId(string tableName, string columnNames, string values, string idColumnName = "id");

        public ISqlDialect Update(string tableName, string setColumns);
        public ISqlDialect Update(string tableName, Action<SqlUpdateBuilder> updateBuilder);

        public ISqlDialect Delete(string tableName);

        public ISqlDialect EndStatement();

        public ISqlDialect AppendSql(ReadOnlySpan<char> sql);

        public ISqlDialect Clear();
    }
}