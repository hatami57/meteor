using System;
using System.Threading.Tasks;
using Meteor.Utils;
using Serilog;

namespace Meteor.Operation
{
    public abstract class OperationAsync<TInput>
    {
        public OperationState State { get; private set; } = OperationState.Created;

        protected virtual Task ValidateInputAsync(TInput input) =>
            Task.CompletedTask;

        protected virtual Task<TInput> PrepareInputAsync(TInput input) =>
            Task.FromResult(input);

        protected virtual Task PrepareExecutionAsync(TInput input) =>
            Task.CompletedTask;
        
        protected virtual Task ValidateBeforeExecutionAsync(TInput input) =>
            Task.CompletedTask;
        
        protected abstract Task ExecutionAsync(TInput input);
        
        protected virtual Task ValidateAfterExecutionAsync(TInput input) =>
            Task.CompletedTask;
        
        /// <summary>
        /// This method is called when the operation is executed successfully
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnSuccessAsync(TInput input) =>
            Task.CompletedTask;
        
        protected virtual Task OnErrorAsync(TInput input, Exception e) =>
            Task.CompletedTask;
        
        protected virtual Task FinalizeAsync(TInput input) =>
            Task.CompletedTask;

        public virtual async Task ExecuteAsync(TInput input)
        {
            var operationName = GetType().FullName;

            try
            {
                Log.Debug("start executing {OperationName} operation, with {@Input}", operationName, input);
                
                Log.Verbose("calling {MethodName}", nameof(ValidateInputAsync));
                await ValidateInputAsync(input).ConfigureAwait(false);
                State = OperationState.ValidatedInput;

                Log.Verbose("calling {MethodName}", nameof(PrepareInputAsync));
                input = await PrepareInputAsync(input).ConfigureAwait(false);
                State = OperationState.PreparedInput;

                Log.Verbose("calling {MethodName}", nameof(PrepareExecutionAsync));
                await PrepareExecutionAsync(input).ConfigureAwait(false);
                State = OperationState.PreparedExecution;

                Log.Verbose("calling {MethodName}", nameof(ValidateBeforeExecutionAsync));
                await ValidateBeforeExecutionAsync(input).ConfigureAwait(false);
                State = OperationState.ValidatedBeforeExecution;

                Log.Verbose("calling {MethodName}", nameof(ExecutionAsync));
                await ExecutionAsync(input).ConfigureAwait(false);
                State = OperationState.Executed;

                Log.Verbose("calling {MethodName}", nameof(ValidateAfterExecutionAsync));
                await ValidateAfterExecutionAsync(input).ConfigureAwait(false);

                await Errors.IgnoreAsync(OnSuccessAsync, input).ConfigureAwait(false);
                Log.Debug("operation executed successfully");

                State = OperationState.Succeed;
            }
            catch (Exception e)
            {
                State = OperationState.Failed;
                await Errors.IgnoreAsync(OnErrorAsync, input, e).ConfigureAwait(false);
                Log.Error(e, "operation execution failed");
                throw;
            }
            finally
            {
                Log.Verbose("calling {MethodName}", nameof(FinalizeAsync));
                await FinalizeAsync(input).ConfigureAwait(false);

                Log.Debug("finish executing {OperationName} operation", operationName);
            }
        }

        public Task<OperationResult> TryExecuteAsync(TInput input) =>
            OperationResult.Try(ExecuteAsync, input);
    }
}