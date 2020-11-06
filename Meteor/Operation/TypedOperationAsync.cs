using System.Threading.Tasks;
using Meteor.Utils;

namespace Meteor.Operation
{
    public abstract class OperationAsync<TResult> : OperationAsync
    {
        public TResult Result { get; protected set; }

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