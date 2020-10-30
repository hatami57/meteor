using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Meteor.Database.Sql;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
    public abstract class DbOperationAsync<T> : SharedOperationAsync<T, SharedLazyDbConnection>
    {
        public DbOperationAsync(SharedLazyDbConnection sharedLazyDbConnection)
        {
            UseSharedObject(sharedLazyDbConnection);
        }

        /// <summary>
        /// Ensure execute a function with a transaction.
        /// It use the Transaction object if it already existed,
        /// or create a new one with a given isolation level.
        /// If it creates a new transaction, it always commit after the function execution.
        /// If rollback is needed, the function should throws.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="isolationLevel"></param>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public async Task<TOut> ExecuteInTransactionAsync<TOut>(Func<IDbTransaction, Task<TOut>> func,
            IsolationLevel isolationLevel)
        {
            if (func == null)
                throw Errors.InvalidInput("null_method");

            if (Shared.Transaction != null)
                return await func(Shared.Transaction).ConfigureAwait(false);

            if (Shared.LazyDbConnection == null)
                throw Errors.InvalidOperation("null_lazy_db_connection");

            await using var tx = await Shared.LazyDbConnection.BeginTransactionAsync(isolationLevel)
                .ConfigureAwait(false);
            try
            {
                Shared.Transaction = tx;
                var res = await func(tx).ConfigureAwait(false);
                Shared.Transaction = null;

                await tx.CommitAsync().ConfigureAwait(false);

                return res;
            }
            catch
            {
                Shared.Transaction = null;

                await tx.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }

        protected SqlGenerator NewSql(string sql = "") =>
            new SqlGenerator(Shared.LazyDbConnection, sql, this, Shared.Transaction);

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql) =>
            await (await Shared.LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryMultipleAsync(sql, this, Shared.Transaction).ConfigureAwait(false);

        protected async Task<IEnumerable<TModel>> QueryAsync<TModel>(string sql) =>
            await (await Shared.LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync<TModel>(sql, this, Shared.Transaction).ConfigureAwait(false);

        protected async Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TReturn>(string sql,
            Func<TModel1, TModel2, TReturn> func, string splitOn = "Id") =>
            await (await Shared.LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync(sql, func, this, Shared.Transaction, splitOn: splitOn).ConfigureAwait(false);

        protected async Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TModel3, TReturn>(string sql,
            Func<TModel1, TModel2, TModel3, TReturn> func, string splitOn = "Id") =>
            await (await Shared.LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync(sql, func, this, Shared.Transaction, splitOn: splitOn).ConfigureAwait(false);

        protected async Task<TModel> QueryFirstOrDefaultAsync<TModel>(string sql) =>
            await (await Shared.LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryFirstOrDefaultAsync<TModel>(sql, this, Shared.Transaction).ConfigureAwait(false);

        protected async Task<int> ExecuteAsync(string sql) =>
            await (await Shared.LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .ExecuteAsync(sql, this, Shared.Transaction).ConfigureAwait(false);

        protected async Task<TModel> ExecuteScalarAsync<TModel>(string sql) =>
            await (await Shared.LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .ExecuteScalarAsync<TModel>(sql, this, Shared.Transaction).ConfigureAwait(false);

        protected Task<T> QueryFirstOrDefaultAsync(string sql) =>
            QueryFirstOrDefaultAsync<T>(sql);

        protected Task<T> ExecuteScalarAsync(string sql) =>
            ExecuteScalarAsync<T>(sql);
    }
}