using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Meteor.Database;
using Meteor.Database.Sql;
using Meteor.Utils;

namespace Meteor.Message.Db
{
    public abstract class DbMessageAsync<T> : MessageAsync<T>
    {
        protected LazyDbConnection LazyDbConnection { get; private set; }
        protected IDbTransaction Transaction { get; private set; }

        public DbMessageAsync(LazyDbConnection lazyDbConnection)
        {
            LazyDbConnection = lazyDbConnection;
            UseLazyDbConnection(lazyDbConnection);
            UseDbTransaction(null);
        }

        public DbMessageAsync() : this(null)
        {
        }

        public DbMessageAsync<T> UseLazyDbConnection(LazyDbConnection lazyDbConnection)
        {
            LazyDbConnection = lazyDbConnection;
            return this;
        }
        
        public DbMessageAsync<T> UseDbTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
            return this;
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
        public async Task<TOut> ExecuteInTransactionAsync<TOut>(Func<IDbTransaction, Task<TOut>> func, IsolationLevel isolationLevel)
        {
            if (func == null)
                throw Errors.InvalidInput("null_method");
            
            if (Transaction != null)
                return await func(Transaction).ConfigureAwait(false);

            await using var tx = await LazyDbConnection.BeginTransactionAsync(isolationLevel).ConfigureAwait(false);
            try
            {
                Transaction = tx;
                var res = await func(tx).ConfigureAwait(false);
                await tx.CommitAsync().ConfigureAwait(false);
                return res;
            }
            catch
            {
                await tx.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }
        
        public Task<TOut> ExecuteDbMessageAsync<TOut>(DbMessageAsync<TOut> message, IDbTransaction tx = null) => 
            LazyDbConnection.ExecuteDbMessageAsync(message, tx ?? Transaction);

        protected SqlGenerator NewSql(string sql = "", IDbTransaction transaction = null) =>
            new SqlGenerator(LazyDbConnection, sql, this, transaction ?? Transaction);

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, IDbTransaction transaction = null) =>
            await (await LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryMultipleAsync(sql, this, transaction ?? Transaction).ConfigureAwait(false);
        
        protected async Task<IEnumerable<TModel>> QueryAsync<TModel>(string sql, IDbTransaction transaction = null) =>
            await (await LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync<TModel>(sql, this, transaction ?? Transaction).ConfigureAwait(false);

        protected async Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TReturn>(string sql,
            Func<TModel1, TModel2, TReturn> func, IDbTransaction transaction = null, string splitOn = "Id") =>
            await (await LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync(sql, func, this, transaction ?? Transaction, splitOn: splitOn).ConfigureAwait(false);
        
        protected async Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TModel3, TReturn>(string sql,
            Func<TModel1, TModel2, TModel3, TReturn> func, IDbTransaction transaction = null, string splitOn = "Id") =>
            await (await LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync(sql, func, this, transaction ?? Transaction, splitOn: splitOn).ConfigureAwait(false);

        protected async Task<TModel> QueryFirstOrDefaultAsync<TModel>(string sql, IDbTransaction transaction = null) =>
            await (await LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryFirstOrDefaultAsync<TModel>(sql, this, transaction ?? Transaction).ConfigureAwait(false);

        protected async Task<int> ExecuteAsync(string sql, IDbTransaction transaction = null) =>
            await (await LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .ExecuteAsync(sql, this, transaction ?? Transaction).ConfigureAwait(false);

        protected async Task<TModel> ExecuteScalarAsync<TModel>(string sql, IDbTransaction transaction = null) =>
            await (await LazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .ExecuteScalarAsync<TModel>(sql, this, transaction ?? Transaction).ConfigureAwait(false);

        protected Task<T> QueryFirstOrDefaultAsync(string sql, IDbTransaction transaction = null) =>
            QueryFirstOrDefaultAsync<T>(sql, transaction ?? Transaction);

        protected Task<T> ExecuteScalarAsync(string sql, IDbTransaction transaction = null) =>
            ExecuteScalarAsync<T>(sql, transaction ?? Transaction);
    }
}