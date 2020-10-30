using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Meteor.Operation.Db;
using Meteor.Utils;

namespace Meteor.Database
{
    public class LazyDbConnection : IAsyncDisposable, IDisposable
    {
        private readonly IDbConnectionFactory _dbFactory;
        private bool _disposed;
        private DbConnection? _db;

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