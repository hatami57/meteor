using System.Threading.Tasks;
using Meteor.Operation;
using Xunit;

namespace Meteor.Test.Operation
{
    public class GenericOperationResultTest
    {
        [Fact]
        public void DefaultValuesAreCorrect()
        {
            var op = new OperationResult<int>();

            Assert.False(op.Success);
            Assert.Null(op.Error);
            Assert.Equal(default, op.Result);
        }

        [Fact]
        public async Task CreateSuccessfulResultByTask()
        {
            const int result = 10;
            var op = await OperationResultFactory.Try(() => 
                Task.FromResult(result));

            Assert.True(op.Success);
            Assert.Null(op.Error);
            Assert.Equal(result, op.Result);
        }

        [Fact]
        public async Task CreateSuccessfulResultByOperationResultTask()
        {
            const int result = 10;
            var op = await OperationResultFactory.Try(() =>
                OperationResultFactory.Try(() => 
                    OperationResultFactory.Try(() => 
                        Task.FromResult(result))));

            Assert.True(op.Success);
            Assert.Null(op.Error);
            Assert.Equal(result, op.Result);
        }
    }
}