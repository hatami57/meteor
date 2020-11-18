using System.Threading.Tasks;
using Meteor.Operation;
using Meteor.Utils;
using Xunit;

namespace Meteor.Test.Operation.OperationResult
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
        public async Task CreateErrorResultByTask()
        {
            var op = await OperationResultFactory.Try(() =>
            {
                const int x = 10;
                if (x < 20) throw Errors.InternalError("intentional_error");
                return Task.FromResult(x);
            });

            Assert.False(op.Success);
            Assert.NotNull(op.Error);
            Assert.Equal(default, op.Result);
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

        [Fact]
        public async Task CreateErrorResultByOperationResultTask()
        {
            var op = await OperationResultFactory.Try(() =>
                OperationResultFactory.Try(() => 
                    OperationResultFactory.Try(() =>
                    {
                        const int x = 10;
                        if (x < 20) throw Errors.InternalError("intentional_error");
                        return Task.FromResult(x);
                    })));

            Assert.False(op.Success);
            Assert.NotNull(op.Error);
            Assert.Equal(default, op.Result);
        }
    }
}