using System;
using System.Linq;
using System.Reflection;
using Conventional;
using Conventional.Conventions;
using Microsoft.Extensions.Logging;

namespace andymac4182.Reference.Tests.Conventions.Specifications
{
    public class MustOnlyUseSerilogILoggerNotMELILogger : ConventionSpecification
    {
        protected override string FailureMessage => "Must not use Microsoft.Extensions.Logging ILogger";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            var anyConstructorsUsingMel = constructors
                .SelectMany(c => c.GetParameters())
                .Any(c => 
                    c.ParameterType == typeof(ILogger) 
                    || (c.ParameterType.IsGenericType && c.ParameterType.GetGenericTypeDefinition() == typeof(ILogger<>))
                );

            return anyConstructorsUsingMel ? ConventionResult.NotSatisfied(type.FullName, FailureMessage) : ConventionResult.Satisfied(type.FullName);
        }
    }
}