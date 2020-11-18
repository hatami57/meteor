using System.Threading.Tasks;

namespace Meteor.Operation
{
    public delegate Task OperationLoggerAsync(IOperationAsync operation);
    public delegate Task OperationLoggerAsync<TInput, TOutput>(IOperationAsync<TInput, TOutput> operation);
    
    public static class DefaultOperationSettings
    {
        public static bool LogInput { get; set; }
        public static OperationLoggerAsync? LoggerAsync { get; set; }
    }
}