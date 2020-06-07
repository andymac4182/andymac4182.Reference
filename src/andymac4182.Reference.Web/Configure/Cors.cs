using Microsoft.Extensions.DependencyInjection;

namespace andymac4182.Reference.Web.Configure
{
    public static class Cors
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins("https://*.qld.gov.au")
                            .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
                });

            return serviceCollection;
        }
    }
}