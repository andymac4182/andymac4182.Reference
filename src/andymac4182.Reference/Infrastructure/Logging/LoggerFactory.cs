using System.Linq;
using System.Reflection;
using andymac4182.Reference.Infrastructure.Extensions;
using andymac4182.Reference.Infrastructure.Logging;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;

// ReSharper disable once CheckNamespace
namespace Serilog
{
    public static class LoggerFactory
    {
        public static LoggerConfiguration SetupCustomLogger(this LoggerConfiguration loggerConfiguration,
            Assembly assembly,
            EnvironmentTypeSetting environmentType,
            SeqServerUriSetting seqServerUri,
            SeqServerApiKeySetting seqServerApiKey)
        {
            var assemblyName = assembly.GetName().Name;
            var infoVerAttr = (AssemblyInformationalVersionAttribute)assembly
                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).FirstOrDefault();
            var assemblyVersion = infoVerAttr?.InformationalVersion;

            var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose);

            loggerConfiguration
                .MinimumLevel.ControlledBy(levelSwitch)
                .If(environmentType.IsProduction(), _ => _.MinimumLevel.Override("Microsoft", LogEventLevel.Warning))
                .If(environmentType.IsProduction(), _ => _.MinimumLevel.Override("System", LogEventLevel.Warning))
                .If(environmentType.IsProduction(), _ => _.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)) // Log SQL Queries from EF
                .Destructure.ToMaximumDepth(4)
                .Destructure.ToMaximumCollectionCount(10)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithExceptionDetails()
                .Enrich.WithCorrelationIdHeader()
                .Enrich.WithProperty("ApplicationName", assemblyName)
                .Enrich.WithProperty("ApplicationVersion", assemblyVersion)
                .Enrich.WithProperty("EnvironmentType", environmentType.Value.ToString())
                .If(!environmentType.IsProduction(),
                    loggerConfiguration => loggerConfiguration.WriteTo.Console()
                )
                .WriteTo.Seq(
                    seqServerUri.Value,
                    apiKey: seqServerApiKey.Value,
                    controlLevelSwitch: levelSwitch);
            
            return loggerConfiguration;
        }
    }
}
