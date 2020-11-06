using System;

namespace Meteor.Database.SqlDialect
{
    public interface ISqlDialect
    {
        public string SqlText { get; set; }
        
        public ISqlDialect Select(string tableName, string columnNames = "*");
        public ISqlDialect InnerJoin(string tableName, string onClause);
        public ISqlDialect LeftJoin(string tableName, string onClause);
        public ISqlDialect RightJoin(string tableName, string onClause);
        public ISqlDialect FullJoin(string tableName, string onClause);
        public ISqlDialect Where(string where);
        public ISqlDialect Where(Action<ISqlDialectWhereBuilder> whereBuilder);
        public ISqlDialect GroupBy(string columnNames);
        public ISqlDialect Having(string having);
        public ISqlDialect OrderBy(string columnNames);
        public ISqlDialect Offset(string? offset, string? fetchFirst);

        public ISqlDialect Insert(string tableName, string columnNames, string values);
        public ISqlDialect InsertReturnId(string tableName, string columnNames, string values);

        public ISqlDialect Update(string tableName, string setColumns);
        public ISqlDialect Update(string tableName, Action<ISqlDialectUpdateBuilder> updateBuilder);

        public ISqlDialect Delete(string tableName);

        public ISqlDialect EndStatement();

        public ISqlDialect AppendSql(string sql);

        public ISqlDialect Clear();
    }
}