using System.Threading.Tasks;
using Meteor.Operation;
using Meteor.Utils;
using Xunit;

namespace Meteor.Test.Operation.OperationResult
{
    public class OperationResultTest
    {
        [Fact]
        public void DefaultValuesAreCorrect()
        {
            var op = new Meteor.Operation.OperationResult();

            Assert.False(op.Success);
            Assert.Null(op.Error);
        }

        [Fact]
        public async Task CreateSuccessfulResultByTask()
        {
            var op = await OperationResultFactory.Try(() => Task.CompletedTask);

            Assert.True(op.Success);
            Assert.Null(op.Error);
        }

        [Fact]
        public async Task CreateErrorResultByTask()
        {
            var op = await OperationResultFactory.Try(() => 
                throw Errors.InternalError("intentional_error"));

            Assert.False(op.Success);
            Assert.NotNull(op.Error);
        }

        [Fact]
        public async Task CreateSuccessfulResultByOperationResultTask()
        {
            var op = await OperationResultFactory.Try(() => 
                OperationResultFactory.Try(() => 
                    Task.CompletedTask));

            Assert.True(op.Success);
            Assert.Null(op.Error);
        }

        [Fact]
        public async Task CreateErrorResultByOperationResultTask()
        {
            var op = await OperationResultFactory.Try(() =>
                OperationResultFactory.Try(() => 
                    throw Errors.InternalError("intentional_error")));

            Assert.False(op.Success);
            Assert.NotNull(op.Error);
        }
    }
}