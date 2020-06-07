using System.Reflection;
using andymac4182.Reference.Infrastructure.Logging;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace andymac4182.Reference.Service.Configure
{
    public static class SerilogConfigure
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder builder, Assembly thisAssembly)
        {
            builder.UseSerilog((context, loggerConfiguration) =>
            {
                var environmentTypeSetting = context.Configuration.CustomRead<EnvironmentTypeSetting>();
                var seqServerUri = context.Configuration.CustomRead<SeqServerUriSetting>();
                var seqServerApiKey = context.Configuration.CustomRead<SeqServerApiKeySetting>();
                loggerConfiguration.SetupCustomLogger(thisAssembly, environmentTypeSetting, seqServerUri,
                    seqServerApiKey);
            });
            
            return builder;
        }
    }
}