using System;
using System.Threading.Tasks;

namespace Meteor.Operation
{
    public interface IOperationAsync
    {
        OperationState State { get; }
        IOperationAsync SetInput(object? input);
        object? Input { get; }
        object? Output { get; }
        Exception? Error { get; }
        
        Task<object?> ExecuteAsync();
        Task<object?> ExecuteAsync(object? input);
        Task<OperationResult> TryExecuteAsync();
        Task<OperationResult> TryExecuteAsync(object? input);
        
        IOperationAsync SetOperationFactory(OperationFactory operationFactory);
        T New<T>() where T : IOperationAsync;
        T New<T>(object? input) where T : IOperationAsync;
        IOperationLoggerAsync? LoggerAsync { get; set; }
    }
    
    public interface IOperationAsync<TInput, TOutput> : IOperationAsync
    {
        IOperationAsync<TInput, TOutput> SetInput(TInput input);
        new TInput Input { get; }
        new TOutput? Output { get; }
        new Task<TOutput?> ExecuteAsync();
        new Task<TOutput?> ExecuteAsync(TInput input);
        new Task<OperationResult<TOutput?>> TryExecuteAsync();
        new Task<OperationResult<TOutput?>> TryExecuteAsync(TInput input);
    }
}