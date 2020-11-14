using Meteor.Utils;
using System.Threading.Tasks;

namespace Meteor.Operation
{
    public abstract class OperationAsync<TIn, TOut> : OperationAsync
    {
		public TIn Input { get; set; }
        public TOut Output { get; protected set; }

        public new async Task<TOut> ExecuteAsync()
        {
            await base.ExecuteAsync().ConfigureAwait(false);
            return Output;
        }

        public new async Task<OperationResult<TOut>> TryExecuteAsync()
        {
            var res = await OperationResult.Try(ExecuteAsync).ConfigureAwait(false);
            return new OperationResult<TOut>(res.Success, Output, res.Error);
        }
    }
}