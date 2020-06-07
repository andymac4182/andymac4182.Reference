using andymac4182.Reference.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using NimbleConfig.DependencyInjection.Aspnetcore;

namespace andymac4182.Reference.Service.Configure
{
    public static class ConfigurationSettings
    {
        public static IServiceCollection AddCustomConfigurationSettings(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddConfigurationSettings()
                .UsingOptionsIn(NimbleConfigExtensions.CustomConfigurationOptions)
                .WithSingletonInstances()
                .ByScanning(new []{andymac4182.Reference.Constants.CoreAssembly, Constants.ServiceAssembly})
                .UsingLogger<NimbleConfigLogger>()
                .AndBuild();
         
            return serviceCollection;
        }
        
    }
}