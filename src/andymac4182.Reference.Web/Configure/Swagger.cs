using andymac4182.Reference.Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace andymac4182.Reference.Web.Configure
{
    public static class Swagger
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "ToDo API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = "https://twitter.com/spboyer"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            });
            
            return serviceCollection;
        }
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder applicationBuilder, EnvironmentTypeSetting environmentType)
        {
            if (environmentType.IsNonProduction())
            {
                applicationBuilder.UseOpenApi().UseSwaggerUi3();
            }

            return applicationBuilder;
        }
    }
}