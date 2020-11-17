using Microsoft.Extensions.DependencyInjection;

namespace Meteor.Operation
{
    public static class OperationExtensions
    {
        public static IServiceCollection AddOperationFactory(this IServiceCollection services)
        {
            services.AddScoped<OperationFactory>();
            
            return services;
        }
    }
}