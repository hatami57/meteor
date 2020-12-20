using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Meteor.Logger
{
    public static class DefaultLogger
    {
        public static LoggerConfiguration Config(string appName, string logFilePath = "logs/log-.txt", long fileSizeLimitMb = 512, int retainedFileCountLimit = 1) =>
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.WithProperty("App", appName)
                .Enrich.WithExceptionDetails()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(logFilePath,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: fileSizeLimitMb * 1024 * 1024,
                    retainedFileCountLimit: retainedFileCountLimit,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");
    }
}
