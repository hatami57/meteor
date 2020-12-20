using System;
using System.Threading.Tasks;
using Meteor.Logger;
using Meteor.Utils;
using Serilog;

namespace Meteor.Operation
{
    public abstract class OperationAsync : OperationAsync<NoType, NoType>
    {
    }

    public abstract class InOperationAsync<TInput> : OperationAsync<TInput, NoType>
    {
    }

    public abstract class OutOperationAsync<TOutput> : OperationAsync<NoType, TOutput>
    {
    }

    public abstract class OperationAsync<TInput, TOutput> : IOperationAsync<TInput, TOutput>
    {
        object? IOperationAsync.Input => Input;
        object? IOperationAsync.Output => Output;
        private TInput? _input;

        public TInput Input
        {
            get => (TInput) _input!;
            private set => _input = value;
        }

        public TOutput? Output { get; protected set; }
        public OperationState State { get; private set; } = OperationState.Created;
        public Exception? Error { get; private set; }
        public IOperationLoggerAsync? LoggerAsync { get; set; }

        protected OperationFactory? OperationFactory { get; set; }

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

        protected virtual Task LogAsync() =>
            LoggerAsync?.LogAsync(this)
            ?? DefaultOperationSettings.LoggerAsync?.Invoke(this)
            ?? Task.CompletedTask;

        protected virtual Task FinalizeAsync() =>
            Task.CompletedTask;

        public virtual IOperationAsync SetOperationFactory(OperationFactory operationFactory)
        {
            OperationFactory = operationFactory;
            return this;
        }

        public T New<T>() where T : IOperationAsync
        {
            if (OperationFactory == null)
                throw Errors.InvalidOperation("OperationFactory==null");

            return OperationFactory.New<T>();
        }

        public T New<T>(object? input) where T : IOperationAsync =>
            (T) New<T>().SetInput(input);

        public IOperationAsync SetInput(object? input)
        {
            if (input is TInput x)
                return SetInput(x);

            throw Errors.InvalidInput("invalid_input_type");
        }

        public IOperationAsync<TInput, TOutput> SetInput(TInput input)
        {
            Input = input;
            return this;
        }

        async Task<object?> IOperationAsync.ExecuteAsync() =>
            await ExecuteAsync().ConfigureAwait(false);

        Task<object?> IOperationAsync.ExecuteAsync(object? input) =>
            SetInput(input).ExecuteAsync();

        async Task<OperationResult> IOperationAsync.TryExecuteAsync() =>
            await TryExecuteAsync().ConfigureAwait(false);

        Task<OperationResult> IOperationAsync.TryExecuteAsync(object? input) =>
            SetInput(input).TryExecuteAsync();

        public virtual async Task<TOutput?> ExecuteAsync()
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
                Error = e;
                State = OperationState.Failed;
                await Errors.IgnoreAsync(OnErrorAsync, e).ConfigureAwait(false);
                Log.Error(e, "operation execution failed");
                throw;
            }
            finally
            {
                Log.Verbose("calling {MethodName}", nameof(FinalizeAsync));
                await FinalizeAsync().ConfigureAwait(false);

                await Errors.IgnoreAsync(LogAsync).ConfigureAwait(false);
                Log.Debug("finish executing {OperationName} operation", operationName);
            }
        }

        public Task<TOutput?> ExecuteAsync(TInput input) =>
            SetInput(input).ExecuteAsync();

        public Task<OperationResult<TOutput?>> TryExecuteAsync() =>
            OperationResultFactory.Try(ExecuteAsync);

        public Task<OperationResult<TOutput?>> TryExecuteAsync(TInput input) =>
            SetInput(input).TryExecuteAsync();
    }
}