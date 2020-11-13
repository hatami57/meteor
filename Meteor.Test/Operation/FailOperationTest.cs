using System;
using System.Threading.Tasks;
using Meteor.Operation;
using Meteor.Utils;
using Xunit;

namespace Meteor.Test.Operation
{
    public class FailOperationTest : OperationAsync
    {
        protected override Task<OperationAsync> PreparePropertiesAsync()
        {
            Assert.Equal(OperationState.Created, State);

            return base.PreparePropertiesAsync();
        }
        
        protected override Task ValidatePropertiesAsync()
        {
            Assert.Equal(OperationState.PreparedProperties, State);
            
            return base.ValidatePropertiesAsync();
        }
        
        protected override Task PrepareExecutionAsync()
        {
            Assert.Equal(OperationState.ValidatedProperties, State);
            
            return base.PrepareExecutionAsync();
        }

        protected override Task ValidateBeforeExecutionAsync()
        {
            Assert.Equal(OperationState.PreparedExecution, State);
            
            return base.ValidateBeforeExecutionAsync();
        }

        protected override Task ExecutionAsync()
        {
            throw Errors.InternalError();
        }

        protected override Task FinalizeAsync()
        {
            Assert.Equal(OperationState.Failed, State);
            
            return Task.CompletedTask;
        }

        protected override Task OnSuccessAsync()
        {
            // here should not be called
            throw Errors.InvalidOperation();
        }

        protected override Task OnErrorAsync(Exception e)
        {
            Assert.Equal(OperationState.Failed, State);
            
            return Task.CompletedTask;
        }

        [Fact]
        public Task Test()
        {
            return Assert.ThrowsAsync<Error>(new FailOperationTest().ExecuteAsync);
        }
    }
}
