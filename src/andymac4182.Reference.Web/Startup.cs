using System.Reflection;
using CorrelationId;
using andymac4182.Reference.Infrastructure.Logging;
using andymac4182.Reference.Web.Configure;
using andymac4182.Reference.Web.Infrastructure.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using andymac4182.Reference.Infrastructure.Masstransit;

namespace andymac4182.Reference.Web
{
    public class Startup
    {
        private readonly Assembly _thisAssembly = typeof(Program).Assembly;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomCors()
                .AddCustomCorrelationId()
                .AddCustomMasstransit(_thisAssembly, false)
                .AddCustomSwagger()
                .AddCustomConfigurationSettings()
                .AddMvc(options =>
                {
                    options.Filters.Add<HandleAjaxExceptionsFilter>();
                    options.Filters.Add<ValidateModelStateFilter>();
                })
                .AddFeatureFolders()
                .AddAreaFeatureFolders()
                ;
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var environmentType = _configuration.CustomRead<EnvironmentTypeSetting>();
            
            app
                .UseCorrelationId()
                .UseCors()
                .UseCustomExceptionHandling(environmentType)
                .UseCustomSecurityHeaders()
                .UseHttpsRedirection()
                .UseCustomSerilog()
                .UseRouting()
                .UseCustomSwagger(environmentType)
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/health");
                })
                ;
        }
    }
}
