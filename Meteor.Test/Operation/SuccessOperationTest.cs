using System;
using System.Threading.Tasks;
using Meteor.Operation;
using Meteor.Utils;
using Xunit;

namespace Meteor.Test.Operation
{
    public class SuccessOperationTest : OperationAsync
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
            Assert.Equal(OperationState.ValidatedBeforeExecution, State);

            return Task.CompletedTask;
        }

        protected override Task ValidateAfterExecutionAsync()
        {
            Assert.Equal(OperationState.Executed, State);
            
            return base.ValidateAfterExecutionAsync();
        }

        protected override Task FinalizeAsync()
        {
            Assert.Equal(OperationState.Succeed, State);
            
            return Task.CompletedTask;
        }

        protected override Task OnSuccessAsync()
        {
            Assert.Equal(OperationState.Succeed, State);

            return Task.CompletedTask;
        }

        protected override Task OnErrorAsync(Exception e)
        {
            // here should not be called
            throw Errors.InvalidOperation();
        }

        [Fact]
        public Task Test()
        {
            return new SuccessOperationTest().ExecuteAsync();
        }
    }
}
