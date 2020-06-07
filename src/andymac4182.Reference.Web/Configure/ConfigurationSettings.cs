using andymac4182.Reference.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using NimbleConfig.DependencyInjection.Aspnetcore;

namespace andymac4182.Reference.Web.Configure
{
    public static class ConfigurationSettings
    {
        public static IServiceCollection AddCustomConfigurationSettings(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddConfigurationSettings()
                .UsingOptionsIn(NimbleConfigExtensions.CustomConfigurationOptions)
                .WithSingletonInstances()
                .ByScanning(new []{Reference.Constants.CoreAssembly, Constants.WebAssembly})
                .UsingLogger<NimbleConfigLogger>()
                .AndBuild();

            return serviceCollection;
        }
        
    }
}