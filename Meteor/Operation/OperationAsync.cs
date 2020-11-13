using System;
using System.Threading.Tasks;
using Meteor.Utils;
using Serilog;

namespace Meteor.Operation
{
    public abstract class OperationAsync
    {
        public OperationState State { get; private set; } = OperationState.Created;

        protected virtual Task<OperationAsync> PreparePropertiesAsync() =>
            Task.FromResult(this);
        
        protected virtual Task ValidatePropertiesAsync() =>
            Task.CompletedTask;
        
        protected virtual Task PrepareExecutionAsync() =>
            Task.CompletedTask;
        
        protected virtual Task ValidateBeforeExecutionAsync() =>
            Task.CompletedTask;
        
        protected abstract Task ExecutionAsync();
        
        protected virtual Task ValidateAfterExecutionAsync() =>
            Task.CompletedTask;
        
        /// <summary>
        /// This method is called when the operation is executed successfully
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnSuccessAsync() =>
            Task.CompletedTask;
        
        protected virtual Task OnErrorAsync(Exception e) =>
            Task.CompletedTask;
        
        protected virtual Task FinalizeAsync() =>
            Task.CompletedTask;

        public virtual async Task ExecuteAsync()
        {
            OperationAsync? operation = null;
            var operationName = GetType().FullName;

            try
            {
                Log.Debug("start executing {OperationName} operation, with {@Properties}", operationName, this);

                Log.Verbose("calling {MethodName}", nameof(PreparePropertiesAsync));
                operation = await PreparePropertiesAsync().ConfigureAwait(false);
                State = OperationState.PreparedProperties;

                Log.Verbose("calling {MethodName}", nameof(ValidatePropertiesAsync));
                await operation.ValidatePropertiesAsync().ConfigureAwait(false);
                State = OperationState.ValidatedProperties;

                Log.Verbose("calling {MethodName}", nameof(PrepareExecutionAsync));
                await operation.PrepareExecutionAsync().ConfigureAwait(false);
                State = OperationState.PreparedExecution;

                Log.Verbose("calling {MethodName}", nameof(ValidateBeforeExecutionAsync));
                await operation.ValidateBeforeExecutionAsync().ConfigureAwait(false);
                State = OperationState.ValidatedBeforeExecution;

                Log.Verbose("calling {MethodName}", nameof(ExecutionAsync));
                await operation.ExecutionAsync().ConfigureAwait(false);
                State = OperationState.Executed;

                Log.Verbose("calling {MethodName}", nameof(ValidateAfterExecutionAsync));
                await operation.ValidateAfterExecutionAsync().ConfigureAwait(false);

                await Errors.IgnoreAsync(operation.OnSuccessAsync).ConfigureAwait(false);
                Log.Debug("operation executed successfully");

                State = OperationState.Succeed;
            }
            catch (Exception e)
            {
                State = OperationState.Failed;
                var onErrorAsync = operation != null ? operation.OnErrorAsync : new Func<Exception, Task>(OnErrorAsync);
                await Errors.IgnoreAsync(onErrorAsync, e).ConfigureAwait(false);
                Log.Error(e, "operation execution failed");
                throw;
            }
            finally
            {
                Log.Verbose("calling {MethodName}", nameof(FinalizeAsync));
                await (operation?.FinalizeAsync() ?? FinalizeAsync()).ConfigureAwait(false);

                Log.Debug("finish executing {OperationName} operation", operationName);
            }
        }

        public Task<OperationResult> TryExecuteAsync() =>
            OperationResult.Try(ExecuteAsync);
    }
}