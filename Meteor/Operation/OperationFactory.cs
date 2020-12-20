using System;
using System.Threading.Tasks;
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

        public T? GetService<T>() => _serviceProvider.GetService<T>();

        public T New<T>(params object[] parameters) where T : IOperationAsync
        {
            var op = ActivatorUtilities.CreateInstance<T>(_serviceProvider, parameters);
            op.SetOperationFactory(this).LoggerAsync = _serviceProvider.GetService<IOperationLoggerAsync>();
            return op;
        }

        public Task<object?> ExecuteAsync<TOperation>() where TOperation : IOperationAsync =>
            New<TOperation>().ExecuteAsync();

        public Task<object?> ExecuteAsync<TOperation>(object? input) where TOperation : IOperationAsync =>
            New<TOperation>().ExecuteAsync(input);

        public Task<TOutput?> ExecuteAsync<TOperation, TInput, TOutput>(TInput input)
            where TOperation : OperationAsync<TInput, TOutput> =>
            New<TOperation>().ExecuteAsync(input);

        public Task<TOutput?> ExecuteAsync<TOperation, TOutput>() where TOperation : OperationAsync<NoType, TOutput> =>
            New<TOperation>().ExecuteAsync();

        public Task<OperationResult<TOutput?>> TryExecuteAsync<TOperation, TInput, TOutput>(TInput input)
            where TOperation : OperationAsync<TInput, TOutput> =>
            New<TOperation>().TryExecuteAsync(input);

        public Task<OperationResult<TOutput?>> TryExecuteAsync<TOperation, TOutput>()
            where TOperation : OperationAsync<NoType, TOutput> =>
            New<TOperation>().TryExecuteAsync();
    }
}