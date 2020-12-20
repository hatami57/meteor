using Microsoft.Extensions.DependencyInjection;

namespace Meteor.Operation
{
    public static class OperationExtensions
    {
        public static IServiceCollection AddOperationFactory(this IServiceCollection services)
        {
            return services.AddScoped<OperationFactory>();
        }

        public static IServiceCollection AddScopedOperationLogger<TOperationLoggerFactory>(this IServiceCollection services)
            where TOperationLoggerFactory : class, IOperationLoggerAsync
        {
            return services.AddScoped<IOperationLoggerAsync, TOperationLoggerFactory>();
        }
        
        public static IServiceCollection AddSingletonOperationLogger<TOperationLoggerFactory>(this IServiceCollection services)
            where TOperationLoggerFactory : class, IOperationLoggerAsync
        {
            return services.AddSingleton<IOperationLoggerAsync, TOperationLoggerFactory>();
        }
    }
}