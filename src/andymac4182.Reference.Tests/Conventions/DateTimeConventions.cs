using Conventional;
using Xunit;

namespace andymac4182.Reference.Tests.Conventions
{
    public class DateTimeConventions
    {
        [Fact]
        public void MustNotResolveCurrentTimeViaDateTime()
        {
            MyAssemblies.ExportedTypes
                .MustConformTo(Convention.MustNotResolveCurrentTimeViaDateTime)
                .WithFailureAssertion(AssertEx.Fail);
        }
        
        [Fact]
        public void MustNotUseDateTimeOffsetNow()
        {
            MyAssemblies.ExportedTypes
                .MustConformTo(Convention.MustNotUseDateTimeOffsetNow)
                .WithFailureAssertion(AssertEx.Fail);
        }
    }
}