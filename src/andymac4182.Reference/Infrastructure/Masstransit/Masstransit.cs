using System;
using System.Linq;
using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace andymac4182.Reference.Infrastructure.Masstransit
{
    public static class Masstransit
    {
        public static IServiceCollection AddCustomMasstransit(this IServiceCollection serviceCollection,
            Assembly consumerAssembly, bool registerReceiveEndpoints = true)
        {

            serviceCollection.AddMassTransit(r =>
                    {
                        if (registerReceiveEndpoints)
                        {
                            r.AddConsumers(consumerAssembly);
                        }

                        r.AddBus(context =>
                        {
                            var busControl = Bus.Factory.CreateUsingAzureServiceBus(x =>
                            {
                                var serviceUri = context.Container.GetService<AzureServiceBusConnectionStringSetting>().Value;

                                x.Host(serviceUri);

                                if (!registerReceiveEndpoints) return;
                                
                                var queueName = consumerAssembly.GetName().Name;

                                x.ReceiveEndpoint(queueName, cc =>
                                {
                                    cc.MaxConcurrentCalls = 1;
                                    cc.ConfigureConsumers(context);
                                    cc.ConfigureDeadLetterQueueDeadLetterTransport();
                                    cc.ConfigureDeadLetterQueueErrorTransport();
                                    cc.UseRetry(x => x.Intervals(RetryIntervals()));
                                });
                            });
                            
                            var logger = context.Container.GetService<Serilog.ILogger>();
                            logger
                                .ForContext("ProbeResult", JsonConvert.SerializeObject(busControl.GetProbeResult(), Formatting.Indented))
                                .Information("Masstransit diagnostic information");
                            
                            return busControl;
                        });
                        
                    });

            serviceCollection.AddSingleton<IHostedService, BusHostedService>();

            return serviceCollection;
        }
        
        private static TimeSpan[] RetryIntervals()
        {
            // [200ms, 400ms, 800ms, 1,600ms, 3,200ms]
            return Enumerable
                .Range(1, 5)
                .Select<int, double>(retryCount => Math.Pow(2, retryCount) * 100)
                .Select(TimeSpan.FromMilliseconds)
                .ToArray();
        }
    }
}