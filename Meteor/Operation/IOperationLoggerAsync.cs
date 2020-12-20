using System.Threading.Tasks;

namespace Meteor.Operation
{
    public delegate Task OperationLoggerAsync(IOperationAsync operation);
    
    public interface IOperationLoggerAsync
    {
        Task LogAsync(IOperationAsync operation);
    }
}