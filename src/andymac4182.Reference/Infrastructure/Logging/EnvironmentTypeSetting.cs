using NimbleConfig.Core.Configuration;

namespace andymac4182.Reference.Infrastructure.Logging
{
    public class EnvironmentTypeSetting : ConfigurationSetting<EnvironmentType>
    {
        public bool IsLocal() => Value == EnvironmentType.Local;
        public bool IsNonProduction() => Value != EnvironmentType.Production;
        public bool IsProduction() => Value == EnvironmentType.Production;
    }
    
    public enum EnvironmentType
    {
        Production,
        Testing,
        Development,
        Local,
    }
}