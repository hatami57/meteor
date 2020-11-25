using System.Threading.Tasks;
using Meteor.Logger;

namespace Meteor.Operation
{
    public interface IOperationAsync
    {
        OperationState State { get; }
        IOperationAsync SetInput(object input);
        object? GetInput();
        object? GetOutput();
        
        Task<object?> ExecuteAsync();
        Task<OperationResult> TryExecuteAsync();
        
        IOperationLoggerAsync? LoggerAsync { get; set; }
    }
    
    public interface IOperationAsync<TInput, TOutput> : IOperationAsync
    {
        IOperationAsync<TInput, TOutput> SetInput(TInput input);
        new TInput GetInput();
        new TOutput GetOutput();
        new Task<TOutput> ExecuteAsync();
        new Task<OperationResult<TOutput>> TryExecuteAsync();
    }
}