using System.Data;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
    public class SharedLazyDbConnection
    {
        public LazyDbConnection LazyDbConnection { get; set; }
        public IDbTransaction? Transaction { get; set; }
        
        public SharedLazyDbConnection(LazyDbConnection lazyDbConnection, IDbTransaction? transaction = null)
        {
            LazyDbConnection = lazyDbConnection;
            Transaction = transaction;
        }
        
        public Task<T> ExecuteDbOperationAsync<T>(DbOperationAsync<T> operation) =>
            operation.UseSharedObject(this).ExecuteAsync();

        public Task<OperationResult<T>> TryExecuteDbOperationAsync<T>(DbOperationAsync<T> operation) =>
            operation.UseSharedObject(this).TryExecuteAsync();
    }
}