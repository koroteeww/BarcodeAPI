using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarcodeAPI.Helpers;
using BarcodeAPI.DB;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BarcodeAPI
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

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                SwaggerBaseConfigure(options, "v1");
            });

            //archiver
            services.AddHostedService<BgWorkerArchiver>();
            //SQL
            string conn = Configuration.GetValue<string>("dbConnString");
            //docker
            string docker = Configuration.GetValue<string>("useDocker");
            if (docker=="1")
            {
                conn = Configuration.GetValue<string>("dbConnStringDocker");
            }
            if (docker == "2")
            {
                conn = Configuration.GetValue<string>("dbConnStringProd");
            }
            services.AddDbContext<BarcodeDbContext>
            (options => 
                options.UseSqlServer
                (conn, ef => ef.MigrationsAssembly(typeof(BarcodeDbContext).Assembly.FullName)),
                optionsLifetime: ServiceLifetime.Singleton);
            
            //если эту строку раскомментировать, миграции перестают работать!
            //services.AddScoped<BarcodeDbContext>(provider => provider.GetService<BarcodeDbContext>());


        }

        private void SwaggerBaseConfigure(SwaggerGenOptions options, string version)
        {

            //filter
            //options.SchemaFilter<SwaggerExcludeFilterCore>();

            options.SwaggerDoc("v1", 
                new OpenApiInfo { 
                Title = "BarcodeAPI", 
                Version = version, 
                Description = "Barcode API." 
                });

            options.EnableAnnotations();

            //options.AddSecurityDefinition
            //    (
            //    "JWT",
            //    new OpenApiSecurityScheme
            //    {
            //        Type = SecuritySchemeType.ApiKey,
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Description = "Type into the textbox: Bearer {your JWT token}.",
            //        BearerFormat = "Bearer {your JWT token}"
            //    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BarcodeDbContext dbContext)
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Barcode API.";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Barcode API V1");                
            });

            //SQL
            try
            {
                //migrate
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {                
                throw;                
            }

        }
    }
}
