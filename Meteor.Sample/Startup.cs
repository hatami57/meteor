using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Meteor.AspCore;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Database.SqlDialect.Sqlite;
using Meteor.Database.Sqlite;
using Meteor.Logger;
using Meteor.Operation;
using Meteor.Operation.Db;
using Meteor.Sample.Operations.Db;
using Meteor.Sample.Operations.Db.Models.User.Commands;
using Meteor.Sample.Operations.Db.Models.User.Dto;
using Meteor.Sample.Operations.Logging;
using Meteor.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Meteor.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            Directory.CreateDirectory("data");
            EnvVars.SetDefaultValue(EnvVarKeys.DbUri, "data source=data/main.db");
            
            services.AddOperationFactory()
                .AddDbOperation<SqliteDbConnectionFactory, SqliteDialect>()
                .AddScopedOperationLogger<OperationLogger>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DefaultOperationSettings.LoggerAsync = operation =>
            {
                Log.Information("** INSIDE Default Logger");
                return Task.CompletedTask;
            };

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            using var scope = app.ApplicationServices.CreateScope();
            scope.ServiceProvider.GetRequiredService<OperationFactory>()
                .ExecuteAsync<CreateDatabase>()
                .Wait();
        }
    }
}
