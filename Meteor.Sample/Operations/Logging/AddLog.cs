using System;
using System.Text.Json;
using System.Threading.Tasks;
using Meteor.Operation;
using Meteor.Sample.Operations.Logging.Types;

namespace Meteor.Sample.Operations.Logging
{
    public class AddLog : OperationAsync<ILog, bool>
    {
        protected override Task LoggerAsync()
        {
            return Task.CompletedTask;
        }

        protected override Task ExecutionAsync()
        {
            var type = Input switch
            {
                ILogSelect => "Select",
                ILogInsert => "Insert",
                ILogUpdate => "Update",
                ILogDelete => "Delete",
                _ => "General"
            };
            Console.WriteLine($"[LOG] (UserId: {Input.LogDetails.UserId}) [{type}] => {Input.LogDetails.Result} (Input: {JsonSerializer.Serialize(Input.LogDetails.Input)}, Output: {JsonSerializer.Serialize(Input.LogDetails.Output)})");
            Output = true;
            return Task.CompletedTask;
        }
    }
}