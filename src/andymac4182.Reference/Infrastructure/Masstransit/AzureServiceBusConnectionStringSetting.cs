using NimbleConfig.Core.Configuration;

namespace andymac4182.Reference.Infrastructure.Masstransit
{
    public class AzureServiceBusConnectionStringSetting : ConfigurationSetting<string>
    {
    }

    public class MessageBusSetting : ConfigurationSetting<MessageBusSetting.MessageBusTransports>
    {
        public enum MessageBusTransports
        {
            None,
            AzureServiceBus,
            ActiveMQ
        }
    }
}