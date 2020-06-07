using System.Threading.Tasks;
using andymac4182.Reference.Infrastructure.Masstransit;
using andymac4182.Reference.Service.Configure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;

namespace andymac4182.Reference.Service
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var thisAssembly = typeof(Program).Assembly;

            var builder = new HostBuilder();
            builder
                .ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageCoreServices();
                    b.AddTimers();
                })
                .UseCustomSerilog(thisAssembly)
                .ConfigureServices((hostContext, services) => services
                    .AddCustomConfigurationSettings()
                    .AddCustomMasstransit(thisAssembly)
                    .AddCustomPersistence()
                    .AddSingleton<IClock>(t => SystemClock.Instance)
                    .AddApplicationInsightsTelemetryWorkerService()
                );
            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}