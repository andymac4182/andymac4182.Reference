using Conventional;
using andymac4182.Reference.Tests.Conventions.Specifications;
using Xunit;

namespace andymac4182.Reference.Tests.Conventions
{
    public class LoggingConventions
    {
        [Fact]
        public void MustOnlyUserSerilogILoggerNotMELILogger()
        {
            MyAssemblies.ExportedTypes
                .MustConformTo(new MustOnlyUseSerilogILoggerNotMELILogger())
                .WithFailureAssertion(AssertEx.Fail);
        }
    }
}