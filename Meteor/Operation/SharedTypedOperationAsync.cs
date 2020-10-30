using System.Threading.Tasks;
using Meteor.Utils;

namespace Meteor.Operation
{
    public abstract class SharedOperationAsync<TResult, TShared> : SharedOperationAsync<TShared> where TShared : class?
    {
        public TResult Result { get; protected set; }

        public new SharedOperationAsync<TResult, TShared> UseSharedObject(TShared shared) =>
            (base.UseSharedObject(shared) as SharedOperationAsync<TResult, TShared>)!;

        public Task<T> ShareAndExecuteAsync<T>(SharedOperationAsync<T, TShared> operation) =>
            operation.UseSharedObject(Shared).ExecuteAsync();

        public new async Task<TResult> ExecuteAsync()
        {
            await base.ExecuteAsync().ConfigureAwait(false);
            return Result;
        }

        public new async Task<OperationResult<TResult>> TryExecuteAsync()
        {
            var res = await OperationResult.Try(ExecuteAsync).ConfigureAwait(false);
            return new OperationResult<TResult>(res.Success, Result, res.Error);
        }
    }
}