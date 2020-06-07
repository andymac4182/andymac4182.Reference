using Microsoft.Extensions.Configuration;
using NimbleConfig.Core.Configuration;
using NimbleConfig.Core.Extensions;
using NimbleConfig.Core.Options;

namespace andymac4182.Reference.Infrastructure.Logging
{
    public static class NimbleConfigExtensions
    {
        public static T CustomRead<T>(this IConfiguration configuration) =>
            configuration.QuickReadSetting<T>(CustomConfigurationOptions);
        public static IConfigurationOptions CustomConfigurationOptions => ConfigurationOptions.Create()
            .WithNamingScheme((type, name) => new KeyName("", type.Name.Substring(0, type.Name.Length - 7)));
    }
}