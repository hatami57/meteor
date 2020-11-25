using System.Threading.Tasks;
using Meteor.Operation;

namespace Meteor.Logger
{
    public delegate Task OperationLoggerAsync(IOperationAsync operation);
    
    public interface IOperationLoggerAsync
    {
        Task LogAsync(IOperationAsync operation);
    }
}