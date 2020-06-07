using andymac4182.Reference.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace andymac4182.Reference.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var thisAssembly = typeof(Program).Assembly;
            
            return Host.CreateDefaultBuilder(args)
                .UseSerilog(((context, loggerConfiguration) =>
                {
                    var environmentTypeSetting = context.Configuration.CustomRead<EnvironmentTypeSetting>();
                    var seqServerUri = context.Configuration.CustomRead<SeqServerUriSetting>();
                    var seqServerApiKey = context.Configuration.CustomRead<SeqServerApiKeySetting>();
                    loggerConfiguration.SetupCustomLogger(thisAssembly, environmentTypeSetting, seqServerUri, seqServerApiKey);
                }))
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
