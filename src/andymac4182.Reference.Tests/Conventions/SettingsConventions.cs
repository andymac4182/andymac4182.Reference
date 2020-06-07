using Conventional;
using andymac4182.Reference.Tests.Conventions.Specifications;
using Xunit;

namespace andymac4182.Reference.Tests.Conventions
{
    public class SettingsConventions
    {
        
        [Fact]
        public void AllSettingsClassesMustEndInSetting()
        {
            MyAssemblies.ExportedTypes
                .MustConformTo(new AllSettingsClassesMustEndInSetting())
                .WithFailureAssertion(AssertEx.Fail);
        }
    }
}