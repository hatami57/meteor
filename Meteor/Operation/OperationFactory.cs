using System;
using System.Threading.Tasks;
using Meteor.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Meteor.Operation
{
    public class OperationFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public OperationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Create<T>(params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<T>(_serviceProvider, parameters);
        }

        public Task<TOutput> ExecuteAsync<TOperation, TInput, TOutput>(TInput input) where TOperation : OperationAsync<TInput, TOutput> =>
            Create<TOperation>().SetInput(input).ExecuteAsync();

        public Task<TOutput> ExecuteAsync<TOperation, TOutput>() where TOperation : OperationAsync<NoType, TOutput> =>
            Create<TOperation>().ExecuteAsync();
        
        public Task<OperationResult<TOutput>> TryExecuteAsync<TOperation, TInput, TOutput>(TInput input) where TOperation : OperationAsync<TInput, TOutput> =>
            Create<TOperation>().SetInput(input).TryExecuteAsync();

        public Task<OperationResult<TOutput>> TryExecuteAsync<TOperation, TOutput>() where TOperation : OperationAsync<NoType, TOutput> =>
            Create<TOperation>().TryExecuteAsync();
    }
}