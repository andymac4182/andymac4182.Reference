using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NimbleConfig.DependencyInjection.Aspnetcore;

namespace andymac4182.Reference.Infrastructure.Logging
{
    public static class ConfigurationSettings
    {
        public static IServiceCollection AddCustomConfigurationSettings(this IServiceCollection serviceCollection, params Assembly[] assembliesToScan)
        {
            serviceCollection
                .AddConfigurationSettings()
                .UsingOptionsIn(NimbleConfigExtensions.CustomConfigurationOptions)
                .WithSingletonInstances()
                .ByScanning(assembliesToScan.Concat(new []{andymac4182.Reference.Constants.CoreAssembly}).ToArray())
                .UsingLogger<NimbleConfigLogger>()
                .AndBuild();

            return serviceCollection;
        }
        
    }
}