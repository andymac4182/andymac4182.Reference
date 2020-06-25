using System;
using System.Linq;
using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.ActiveMqTransport;
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
                            IBusControl busControl;

                            var logger = context.Container.GetService<Serilog.ILogger>();
                            var messageBusSetting = context.Container.GetService<MessageBusSetting>();

                            switch (messageBusSetting.Value)
                            {
                                case MessageBusSetting.MessageBusTransports.AzureServiceBus:
                                    busControl = CreateUsingAzureServiceBus(consumerAssembly, registerReceiveEndpoints, context);
                                    break;
                                case MessageBusSetting.MessageBusTransports.ActiveMQ:
                                    busControl = CreateUsingActiveMQ(consumerAssembly, registerReceiveEndpoints, context);
                                    break;
                                default:
                                    logger.Warning("No message bus transport selected");
                                    return default;
                            }

                            logger
                                .ForContext("ProbeResult", JsonConvert.SerializeObject(busControl.GetProbeResult(), Formatting.Indented))
                                .Information("Masstransit diagnostic information");
                            
                            return busControl;
                        });
                        
                    });

            serviceCollection.AddSingleton<IHostedService, BusHostedService>();

            return serviceCollection;
        }

        private static IBusControl CreateUsingAzureServiceBus(Assembly consumerAssembly, bool registerReceiveEndpoints,
            IRegistrationContext<IServiceProvider> context)
        {
            IBusControl busControl;
            busControl = Bus.Factory.CreateUsingAzureServiceBus(x =>
            {
                var serviceUri = context.Container
                    .GetService<AzureServiceBusConnectionStringSetting>().Value;

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
            return busControl;
        }

        private static IBusControl CreateUsingActiveMQ(Assembly consumerAssembly, bool registerReceiveEndpoints,
            IRegistrationContext<IServiceProvider> context)
        {
            IBusControl busControl;
            busControl = Bus.Factory.CreateUsingActiveMq(x =>
            {
                x.Host("localhost", configurator =>
                {
                    configurator.Username("admin");
                    configurator.Password("admin");
                });

                if (!registerReceiveEndpoints) return;

                var queueName = consumerAssembly.GetName().Name.Replace(".", "__");

                x.ReceiveEndpoint(queueName, cc =>
                {
                    cc.ConfigureConsumers(context);
                    cc.UseRetry(x => x.Intervals(RetryIntervals()));
                });
            });
            return busControl;
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