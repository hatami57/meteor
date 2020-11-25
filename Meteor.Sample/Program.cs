using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Meteor.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = Logger.DefaultLogger.Config("Sample").CreateLogger();

            try
            {
                Log.Debug("app is starting");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "app terminated unexpectedly");
            }
            finally
            {
                Log.Information("good bye");
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });
    }
}
