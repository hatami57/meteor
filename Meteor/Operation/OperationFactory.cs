using System;
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
    }
}