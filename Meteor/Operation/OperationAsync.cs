using System;
using System.Threading.Tasks;
using Meteor.Utils;
using Serilog;

namespace Meteor.Operation
{
    public abstract class OperationAsync<TInput, TOutput> : IOperationAsync<TInput, TOutput>
    {
        public TInput Input { get; private set; }
        public TOutput Output { get; protected set; }
        public OperationState State { get; private set; } = OperationState.Created;
        public virtual bool LogInput => DefaultOperationSettings.LogInput;

        protected virtual Task ValidateInputAsync() =>
            Task.CompletedTask;

        protected virtual Task<TInput> PrepareInputAsync() =>
            Task.FromResult(Input);

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

        protected virtual Task LoggerAsync() =>
            DefaultOperationSettings.LoggerAsync?.Invoke(this) ?? Task.CompletedTask;

        protected virtual Task FinalizeAsync() =>
            Task.CompletedTask;

        public IOperationAsync<TInput, TOutput> SetInput(TInput input)
        {
            Input = input;
            return this;
        }

        public IOperationAsync SetInput(object input)
        {
            if (input is TInput x)
                return SetInput(x);

            throw Errors.InvalidInput("invalid_input_type");
        }

        object? IOperationAsync.GetInput() => GetInput();

        object? IOperationAsync.GetOutput() => GetOutput();

        Task IOperationAsync.ExecuteAsync() => ExecuteAsync();

        async Task<OperationResult> IOperationAsync.TryExecuteAsync() =>
            await TryExecuteAsync().ConfigureAwait(false);

        public TInput GetInput() => Input;
        public TOutput GetOutput() => Output;

        public virtual async Task<TOutput> ExecuteAsync()
        {
            var operationName = GetType().FullName;

            try
            {
                Log.Debug("start executing {OperationName} operation, with {@Input}", operationName, Input);

                Log.Verbose("calling {MethodName}", nameof(ValidateInputAsync));
                await ValidateInputAsync().ConfigureAwait(false);
                State = OperationState.ValidatedInput;

                Log.Verbose("calling {MethodName}", nameof(PrepareInputAsync));
                Input = await PrepareInputAsync().ConfigureAwait(false);
                State = OperationState.PreparedInput;

                Log.Verbose("calling {MethodName}", nameof(PrepareExecutionAsync));
                await PrepareExecutionAsync().ConfigureAwait(false);
                State = OperationState.PreparedExecution;

                Log.Verbose("calling {MethodName}", nameof(ValidateBeforeExecutionAsync));
                await ValidateBeforeExecutionAsync().ConfigureAwait(false);
                State = OperationState.ValidatedBeforeExecution;

                Log.Verbose("calling {MethodName}", nameof(ExecutionAsync));
                await ExecutionAsync().ConfigureAwait(false);
                State = OperationState.Executed;

                Log.Verbose("calling {MethodName}", nameof(ValidateAfterExecutionAsync));
                await ValidateAfterExecutionAsync().ConfigureAwait(false);

                await Errors.IgnoreAsync(OnSuccessAsync).ConfigureAwait(false);
                Log.Debug("operation executed successfully");

                State = OperationState.Succeed;
                return Output;
            }
            catch (Exception e)
            {
                State = OperationState.Failed;
                await Errors.IgnoreAsync(OnErrorAsync, e).ConfigureAwait(false);
                Log.Error(e, "operation execution failed");
                throw;
            }
            finally
            {
                Log.Verbose("calling {MethodName}", nameof(FinalizeAsync));
                await FinalizeAsync().ConfigureAwait(false);
                
                await Errors.IgnoreAsync(LoggerAsync).ConfigureAwait(false);
                Log.Debug("finish executing {OperationName} operation", operationName);
            }
        }

        public Task<OperationResult<TOutput>> TryExecuteAsync() =>
            OperationResultFactory.Try(ExecuteAsync);
    }
}