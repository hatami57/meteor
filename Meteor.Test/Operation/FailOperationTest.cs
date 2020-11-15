using System;
using System.Threading.Tasks;
using Meteor.Operation;
using Meteor.Utils;
using Xunit;

namespace Meteor.Test.Operation
{
    public class FailOperationTest : OperationAsync
    {
        protected override Task ValidateInputAsync()
        {
            Assert.Equal(OperationState.Created, State);

            return base.ValidateInputAsync();
        }

        protected override Task<NoType> PrepareInputAsync()
        {
            Assert.Equal(OperationState.ValidatedInput, State);

            return base.PrepareInputAsync();
        }
        
        protected override Task PrepareExecutionAsync()
        {
            Assert.Equal(OperationState.PreparedInput, State);
            
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
