using AspNetCoreRateLimit;
using Contracts;
using Entities.DataTransferObjects;
using Filters.ActionFilters;
using FridgeServer.Authentication;
using FridgeServer.Extensions;
using FridgeServer.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Repository.DataShaping;
using System;
using System.IO;

namespace FridgeServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(),
                "/nlog.config"));

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIisIntegration();
            services.ConfigureLoggerService();
            services.ConfigureSlqContext(Configuration);
            services.ConfigureRepositoryManager();
            services.ConfigureVersioning();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<ValidateFridgeExistsAttribute>();
            services.AddScoped<ValidateFridgeModelForFridgeExistsAttribute>();
            services.AddScoped<IDataShaper<FridgeModelDto>, DataShaper<FridgeModelDto>>();
            services.AddScoped<ValidateMediaTypeAttribute>();
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<FridgeModelLinks>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters()
            .AddCustomCsvFormatter();

            services.AddCustomMediaTypes();

            services.AddMemoryCache();
            services.ConfigureRateLimitingOptions();
            services.AddInMemoryRateLimiting();
            services.AddHttpContextAccessor();
            
            services.ConfigureIdentity();
            services.ConfigureJwt(Configuration);
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();

            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FridgeServer v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "FridgeServer v2");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureExceptionHandler(logger);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}