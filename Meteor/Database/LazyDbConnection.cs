using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Database
{
    public class LazyDbConnection : IAsyncDisposable, IDisposable
    {
        private readonly IDbConnectionFactory _dbFactory;
        private bool _disposed;
        private DbConnection? _db;
        private DbTransaction? _sharedTransaction;

        public LazyDbConnection(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<DbConnection> GetConnectionAsync() =>
            _db ??= await _dbFactory.OpenNewConnectionAsync().ConfigureAwait(false);

        public async ValueTask<DbTransaction> BeginTransactionAsync(IsolationLevel il)
        {
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.BeginTransactionAsync(il).ConfigureAwait(false);
        }

        /// <summary>
        /// Ensure execute a function inside a transaction.
        /// It create a new transaction with a given isolation level, if there is no transaction before.
        /// It automatically commit after the function execution, or rollback when the function throws.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="isolationLevel"></param>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public async Task<TOut> EnsureInTransactionAsync<TOut>(Func<LazyDbConnection, IDbTransaction, Task<TOut>> func,
            IsolationLevel isolationLevel)
        {
            if (func == null)
                throw Errors.InvalidInput("null_func");

            if (_sharedTransaction != null)
                return await func(this, _sharedTransaction).ConfigureAwait(false);

            await using var tx = await BeginTransactionAsync(isolationLevel).ConfigureAwait(false);
            _sharedTransaction = tx;
            try
            {
                var res = await func(this, tx).ConfigureAwait(false);
                _sharedTransaction = null;
                await tx.CommitAsync().ConfigureAwait(false);
                return res;
            }
            catch
            {
                _sharedTransaction = null;
                await tx.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }
        
        public Task<SqlMapper.GridReader> QueryMultipleAsync(string sqlText, object? param) =>
            GetConnectionAsync().Then(db => db.QueryMultipleAsync(sqlText, param, _sharedTransaction));

        public Task<IEnumerable<T>> QueryAsync<T>(string sqlText, object? param) =>
            GetConnectionAsync().Then(db => db.QueryAsync<T>(sqlText, param, _sharedTransaction));

        public Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TReturn>(string sqlText,
            Func<TModel1, TModel2, TReturn> func, object? param, string splitOn = "Id") =>
            GetConnectionAsync().Then(db => db.QueryAsync(sqlText, func, param, _sharedTransaction, splitOn: splitOn));

        public Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TModel3, TReturn>(string sqlText,
            Func<TModel1, TModel2, TModel3, TReturn> func, object? param, string splitOn = "Id") =>
            GetConnectionAsync().Then(db => db.QueryAsync(sqlText, func, param, _sharedTransaction, splitOn: splitOn));

        public Task<T> QueryFirstOrDefaultAsync<T>(string sqlText, object? param) =>
            GetConnectionAsync().Then(db => db.QueryFirstOrDefaultAsync<T>(sqlText, param, _sharedTransaction));

        public Task<int> ExecuteAsync(string sqlText, object? param) =>
            GetConnectionAsync().Then(db => db.ExecuteAsync(sqlText, param, _sharedTransaction));

        public Task<T> ExecuteScalarAsync<T>(string sqlText, object? param) =>
            GetConnectionAsync().Then(db => db.ExecuteScalarAsync<T>(sqlText, param, _sharedTransaction));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _db?.Dispose();

            _disposed = true;
        }

        public ValueTask DisposeAsync()
        {
            if (_db != null && _db is IAsyncDisposable asyncDb)
                return asyncDb.DisposeAsync();

            return default;
        }
    }
}