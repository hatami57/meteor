using System.Collections.Generic;
using System.Threading.Tasks;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
    public static class Extensions
    {
        public static ISqlDialect AddPagination(this ISqlDialect sqlDialect) =>
            sqlDialect.Offset("@Offset", "@Take");

        public static ISqlDialect WhereThisId(this ISqlDialect sqlDialect) =>
            sqlDialect.Where("id=@Id");

        public static ISqlDialect OrderById(this ISqlDialect sqlDialect) =>
            sqlDialect.OrderBy("id ASC");

        public static ISqlDialect SelectPage(this ISqlDialect sqlDialect, string tableName) =>
            sqlDialect.Clear().Select(tableName).OrderById().AddPagination();

        public static ISqlDialect SelectThisId(this ISqlDialect sqlDialect, string tableName) =>
            sqlDialect.Clear().Select(tableName).WhereThisId();

        public static ISqlDialect SelectCount(this ISqlDialect sqlDialect, string tableName) =>
            sqlDialect.Clear().Select(tableName, "COUNT(*)");

        public static ISqlDialect UpdateThisId(this ISqlDialect sqlDialect, string tableName, string setColumns) =>
            sqlDialect.Clear().Update(tableName, setColumns).WhereThisId();

        public static ISqlDialect DeleteThisId(this ISqlDialect sqlDialect, string tableName) =>
            sqlDialect.Clear().Delete(tableName).WhereThisId();

        public static QueryPage<TOutput> CreateQueryPage<TInput, TOutput>(this DbQueryPageAsync<TInput, TOutput> dbOperation, IEnumerable<TOutput> items,
            long totalCount) where TInput : IQueryPageInput
        {
            if (dbOperation == null) throw Errors.InvalidInput("null_db_operation");

            return new QueryPage<TOutput>(items, dbOperation.Input.Page, dbOperation.Input.Take, totalCount);
        }

        public static async Task<QueryPage<TOutput>> SelectQueryPageAsync<TInput, TOutput>(this DbQueryPageAsync<TInput, TOutput> dbOperation,
            ISqlDialect selectItems, ISqlDialect selectCount) where TInput : IQueryPageInput
        {
            var items = await dbOperation.LazyDbConnection
                .QueryAsync<TOutput>(selectItems.AddPagination().SqlText, dbOperation.Input)
                .ConfigureAwait(false);
            var totalCount = await dbOperation.LazyDbConnection
                .ExecuteScalarAsync<long>(selectCount.SqlText, dbOperation.Input)
                .ConfigureAwait(false);
            return new QueryPage<TOutput>(items, dbOperation.Input.Page, dbOperation.Input.Take, totalCount);
        }

        public static async Task<QueryPage<TOutput>> SelectQueryPageAsync<TInput, TOutput>(this DbQueryPageAsync<TInput, TOutput> dbOperation,
            ISqlFactory sqlFactory, string tableName) where TInput : IQueryPageInput
        {
            var items = await dbOperation.LazyDbConnection
                .QueryAsync<TOutput>(sqlFactory.Create().SelectPage(tableName).SqlText, dbOperation.Input)
                .ConfigureAwait(false);
            var totalCount = await dbOperation.LazyDbConnection
                .ExecuteScalarAsync<long>(sqlFactory.Create().SelectCount(tableName).SqlText, dbOperation.Input)
                .ConfigureAwait(false);
            return new QueryPage<TOutput>(items, dbOperation.Input.Page, dbOperation.Input.Take, totalCount);
        }
    }
}