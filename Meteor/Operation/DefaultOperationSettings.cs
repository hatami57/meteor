using System;
using System.Threading.Tasks;

namespace Meteor.Operation
{
    public delegate Task OperationLoggerAsync(OperationAsync operation, object data);
    
    public static class DefaultOperationSettings
    {
        public static bool LogInput { get; set; }
        public static OperationLoggerAsync? LoggerAsync { get; set; }
    }
}