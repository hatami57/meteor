using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
    public static class Extensions
    {
        public static ISqlDialect AddPagination(this ISqlDialect sqlDialect) =>
            sqlDialect.Offset("@Skip", "@Take");

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

        public static QueryPage<T> CreateQueryPage<T>(this DbQueryPageAsync<T> dbOperation, IEnumerable<T> items,
            long totalCount)
        {
            if (dbOperation == null) throw Errors.InvalidInput("null_db_operation");

            return new QueryPage<T>(items, dbOperation.Page, dbOperation.Take, totalCount);
        }

        public static async Task<QueryPage<T>> SelectQueryPageAsync<T>(this DbQueryPageAsync<T> dbOperation,
            ISqlDialect selectItems, ISqlDialect selectCount)
        {
            var items = await dbOperation.LazyDbConnection
                .QueryAsync<T>(selectItems.AddPagination().SqlText, dbOperation)
                .ConfigureAwait(false);
            var totalCount = await dbOperation.LazyDbConnection
                .ExecuteScalarAsync<long>(selectCount.SqlText, dbOperation)
                .ConfigureAwait(false);
            return new QueryPage<T>(items, dbOperation.Page, dbOperation.Take, totalCount);
        }

        public static async Task<QueryPage<T>> SelectQueryPageAsync<T>(this DbQueryPageAsync<T> dbOperation,
            ISqlFactory sqlFactory, string tableName)
        {
            var items = await dbOperation.LazyDbConnection
                .QueryAsync<T>(sqlFactory.Create().SelectPage(tableName).SqlText, dbOperation)
                .ConfigureAwait(false);
            var totalCount = await dbOperation.LazyDbConnection
                .ExecuteScalarAsync<long>(sqlFactory.Create().SelectCount(tableName).SqlText, dbOperation)
                .ConfigureAwait(false);
            return new QueryPage<T>(items, dbOperation.Page, dbOperation.Take, totalCount);
        }
    }
}