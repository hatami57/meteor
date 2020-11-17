using System.Dynamic;
using System.Threading.Tasks;
using Meteor.Utils;

namespace Meteor.Operation
{
    public interface IOperationAsync
    {
        IOperationAsync SetInput(object input);
        object? GetInput();
        object? GetOutput();
        
        Task ExecuteAsync();
        Task<OperationResult> TryExecuteAsync();
    }
    
    public interface IOperationAsync<TInput, TOutput> : IOperationAsync
    {
        OperationState State { get; }

        IOperationAsync<TInput, TOutput> SetInput(TInput input);
        new TInput GetInput();
        new TOutput GetOutput();
        new Task<TOutput> ExecuteAsync();
        new Task<OperationResult<TOutput>> TryExecuteAsync();
    }
}