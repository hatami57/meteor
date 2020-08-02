using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Meteor.Message.Db;
using Meteor.Utils;

namespace Meteor.Database
{
    public class LazyDbConnection : IAsyncDisposable, IDisposable
    {
        private readonly IDbConnectionFactory _dbFactory;
        private bool _disposed = false;
        private DbConnection _db;

        public LazyDbConnection(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<DbConnection> GetConnectionAsync()
        {
            if (_db != null)
                return _db;
            
            _db = await _dbFactory.OpenNewConnectionAsync().ConfigureAwait(false);
            return _db;
        }

        public async ValueTask<DbTransaction> BeginTransactionAsync(IsolationLevel il)
        {
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.BeginTransactionAsync(il).ConfigureAwait(false);
        }

        public Task<T> ExecuteDbMessageAsync<T>(DbMessageAsync<T> message) =>
            message?.UseLazyDbConnection(this).ExecuteAsync();

        public Task<T> ExecuteDbMessageAsync<T>(DbMessageAsync<T> message, IDbTransaction tx) =>
            ExecuteDbMessageAsync(message?.UseDbTransaction(tx));

        public Task<OperationResult<T>> TryExecuteDbMessageAsync<T>(DbMessageAsync<T> message) =>
            message?.UseLazyDbConnection(this).TryExecuteAsync();

        public Task<OperationResult<T>> TryExecuteDbMessageAsync<T>(DbMessageAsync<T> message, IDbTransaction tx) =>
            TryExecuteDbMessageAsync(message?.UseDbTransaction(tx));

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
            {
                _db?.Dispose();
            }

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
