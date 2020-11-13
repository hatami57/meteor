using Meteor.Operation;
using Meteor.Test.Helpers;
using Meteor.Utils;
using System.Threading.Tasks;
using Xunit;

namespace Meteor.Test.Operation
{
    public class OperationThrowTest
    {
        [Fact]
        public async Task ShouldThrowInPrepareProperties()
        {
            var op = new SimpleOperation {ShouldThrowInPrepareProperties = true};

            await Assert.ThrowsAsync<Error>(op.ExecuteAsync);
            Assert.Equal("PreparePropertiesAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Failed, op.State);
            Assert.True(op.OnErrorIsCalled);
            Assert.False(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldThrowInValidateProperties()
        {
            var op = new SimpleOperation {ShouldThrowInValidateProperties = true};

            await Assert.ThrowsAsync<Error>(op.ExecuteAsync);
            Assert.Equal("ValidatePropertiesAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Failed, op.State);
            Assert.True(op.OnErrorIsCalled);
            Assert.False(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldThrowInValidateBeforeExecution()
        {
            var op = new SimpleOperation {ShouldThrowInValidateBeforeExecution = true};

            await Assert.ThrowsAsync<Error>(op.ExecuteAsync);
            Assert.Equal("ValidateBeforeExecutionAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Failed, op.State);
            Assert.True(op.OnErrorIsCalled);
            Assert.False(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldThrowInPrepareExecution()
        {
            var op = new SimpleOperation {ShouldThrowInPrepareExecution = true};

            await Assert.ThrowsAsync<Error>(op.ExecuteAsync);
            Assert.Equal("PrepareExecutionAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Failed, op.State);
            Assert.True(op.OnErrorIsCalled);
            Assert.False(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldThrowInExecution()
        {
            var op = new SimpleOperation {ShouldThrowInExecution = true};

            await Assert.ThrowsAsync<Error>(op.ExecuteAsync);
            Assert.Equal("ExecutionAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Failed, op.State);
            Assert.True(op.OnErrorIsCalled);
            Assert.False(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldThrowInValidateAfterExecution()
        {
            var op = new SimpleOperation {ShouldThrowInValidateAfterExecution = true};

            await Assert.ThrowsAsync<Error>(op.ExecuteAsync);
            Assert.Equal("ValidateAfterExecutionAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Failed, op.State);
            Assert.True(op.OnErrorIsCalled);
            Assert.False(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldThrowInFinalize()
        {
            var op = new SimpleOperation {ShouldThrowInFinalize = true};

            await Assert.ThrowsAsync<Error>(op.ExecuteAsync);
            Assert.Equal("FinalizeAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Succeed, op.State);
            Assert.False(op.OnErrorIsCalled);
            Assert.True(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldThrowInOnSuccess()
        {
            var op = new SimpleOperation {ShouldThrowInOnSuccess = true};

            await op.ExecuteAsync();
            Assert.Equal("OnSuccessAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Succeed, op.State);
            Assert.False(op.OnErrorIsCalled);
            Assert.True(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldThrowInOnError()
        {
            var op = new SimpleOperation {ShouldThrowInOnError = true, ShouldThrowInExecution = true};

            await Assert.ThrowsAsync<Error>(op.ExecuteAsync);
            Assert.Equal("OnErrorAsync", op.ThrowAtMethod);
            Assert.Equal(OperationState.Failed, op.State);
            Assert.True(op.OnErrorIsCalled);
            Assert.False(op.OnSuccessIsCalled);
            Assert.True(op.FinalizeIsCalled);
        }

        [Fact]
        public async Task ShouldExecute()
        {
            var op = new SimpleOperation();

            await op.ExecuteAsync();
        }
    }
}