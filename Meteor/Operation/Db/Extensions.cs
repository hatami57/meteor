using System.Collections.Generic;
using System.Threading.Tasks;
using Meteor.Database.Sql;

namespace Meteor.Operation.Db
{
    public static class Extensions
    {
        public static SqlGenerator AddPagination(this SqlGenerator sqlGenerator) =>
            sqlGenerator.Limit("@Take").Offset("@Skip");
        
        public static SqlGenerator WhereThisId(this SqlGenerator sqlGenerator) =>
            sqlGenerator.Where("id=@Id");
        
        public static SqlGenerator OrderById(this SqlGenerator sqlGenerator) =>
            sqlGenerator.OrderBy("id ASC");

        public static Task<IEnumerable<T>> SelectPageAsync<T>(this SqlGenerator sqlGenerator, string tableName) =>
            sqlGenerator.Select(tableName).OrderById().AddPagination().QueryAsync<T>();
        
        public static Task<T> SelectThisIdAsync<T>(this SqlGenerator sqlGenerator, string tableName) =>
            sqlGenerator.Select(tableName).WhereThisId().QueryFirstOrDefaultAsync<T>();

        public static Task<long> SelectCountAsync(this SqlGenerator sqlGenerator, string tableName) => 
            sqlGenerator.Select(tableName, "COUNT(*)").ExecuteScalarAsync<long>();

        public static Task<T> InsertGetIdPgSqlAsync<T>(this SqlGenerator sqlGenerator, string tableName,
            string fieldNames, string fieldValues) =>
            sqlGenerator.Insert(tableName, fieldNames, fieldValues)
                .Append("RETURNING id;")
                .ExecuteScalarAsync<T>();
        
        public static Task<T> InsertGetIdSqliteAsync<T>(this SqlGenerator sqlGenerator, string tableName,
            string fieldNames, string fieldValues) =>
            sqlGenerator.Insert(tableName, fieldNames, fieldValues)
                .EndStatement()
                .Append("SELECT last_insert_rowid();")
                .ExecuteScalarAsync<T>();
        
        public static Task<int> UpdateThisIdAsync(this SqlGenerator sqlGenerator, string tableName, string setFields) =>
            sqlGenerator.Update(tableName, setFields).WhereThisId().ExecuteAsync();
        
        public static Task<int> DeleteThisIdAsync(this SqlGenerator sqlGenerator, string tableName) =>
            sqlGenerator.Delete(tableName).WhereThisId().ExecuteAsync();

        public static QueryPage<T> CreateQueryPageAsync<T>(this DbQueryPageAsync<T> dbMessage,
            IEnumerable<T> items, long totalCount) =>
            new QueryPage<T>(items, dbMessage.Page, dbMessage.Take, totalCount);
        public static async Task<QueryPage<T>> SelectQueryPageAsync<T>(this DbQueryPageAsync<T> dbMessage,
            SqlGenerator selectItems, SqlGenerator selectCount)
        {
            var items = await selectItems.AddPagination().QueryAsync<T>().ConfigureAwait(false);
            var totalCount = await selectCount.ExecuteScalarAsync<long>().ConfigureAwait(false);
            return dbMessage.CreateQueryPageAsync(items, totalCount);
        }
        public static async Task<QueryPage<T>> SelectQueryPageAsync<T>(this SqlGenerator sqlGenerator, string tableName,
            DbQueryPageAsync<T> dbMessage)
        {
            var items = await sqlGenerator.SelectPageAsync<T>(tableName).ConfigureAwait(false);
            var totalCount = await sqlGenerator.NewSql().SelectCountAsync(tableName).ConfigureAwait(false);
            return new QueryPage<T>(items, dbMessage.Page, dbMessage.Take, totalCount);
        }
    }
}