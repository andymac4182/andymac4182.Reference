using System;
using System.Linq;
using Conventional;
using Conventional.Conventions;
using andymac4182.Reference.Infrastructure.Extensions;
using NimbleConfig.Core.Configuration;

namespace andymac4182.Reference.Tests.Conventions.Specifications
{
    public class AllSettingsClassesMustEndInSetting : ConventionSpecification
    {
        protected override string FailureMessage => "All settings classes must end in setting";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var isSettingsType = (type.InheritsFrom(typeof(ConfigurationSetting<>)) ||
                                  type.GetInterfaces().Any(t => t == typeof(IComplexConfigurationSetting)));
            if (isSettingsType)
            {
                return type.Name.EndsWith("Setting")
                    ? ConventionResult.Satisfied(type.FullName)
                    : ConventionResult.NotSatisfied(type.FullName, FailureMessage);
            }
            
            return ConventionResult.Satisfied(type.FullName);
        }
    }
}