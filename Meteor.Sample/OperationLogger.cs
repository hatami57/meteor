using System;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Operation;
using Meteor.Sample.Operations;
using Meteor.Sample.Operations.Logging;
using Serilog;

namespace Meteor.Sample
{
    public class OperationLogger
    {
        private readonly LazyDbConnection _lazyDbConnection;

        public OperationLogger(LazyDbConnection lazyDbConnection)
        {
            _lazyDbConnection = lazyDbConnection;
        }

        public static Task LogOperation(IOperationAsync operation)
        {
            Log.Information("Operation '{Name}({State})', Input: {@Input}, Output: {@Output}",
                operation.GetType().Name, operation.State, operation.GetInput(), operation.GetOutput());

            return operation is ILog
                ? new AddLog().SetInput(operation).ExecuteAsync()
                : Task.CompletedTask;
        }
    }
}