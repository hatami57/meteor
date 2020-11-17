using System;
using System.Collections.Generic;
using System.IO;
using Meteor.AspCore;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Database.SqlDialect.Sqlite;
using Meteor.Database.Sqlite;
using Meteor.Operation;
using Meteor.Operation.Db;
using Meteor.Sample.Operations.Db;
using Meteor.Sample.Operations.Db.Models.User.Commands;
using Meteor.Sample.Operations.Db.Models.User.Dto;
using Meteor.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddOperationFactory();
            services.AddDbOperation<SqliteDbConnectionFactory, SqliteDialect>();

            new CreateDatabase(new LazyDbConnection(new SqliteDbConnectionFactory()),
                    new SqlFactory<SqliteDialect>())
                .ExecuteAsync().Wait();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
