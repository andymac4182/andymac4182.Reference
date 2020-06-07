using System.Linq;
using Conventional;
using Xunit;

namespace andymac4182.Reference.Tests.Conventions
{
    public class MessageContracts
    {
        [Fact]
        public void PropertiesMustNotHaveSetters()
        {
            MyAssemblies.ExportedTypes
                .Where(t => t.Namespace!.Contains("MessageContracts"))
                .MustConformTo(Convention.PropertiesMustNotHaveSetters)
                .WithFailureAssertion(AssertEx.Fail);
        }
        
        [Fact]
        public void MustHaveAppropriateConstructors()
        {
            MyAssemblies.ExportedTypes
                .Where(t => t.Namespace!.Contains("MessageContracts"))
                .MustConformTo(Convention.MustHaveAppropriateConstructors)
                .WithFailureAssertion(AssertEx.Fail);
        }
        
        [Fact]
        public void AllPropertiesMustBeAssignedDuringConstruction()
        {
            MyAssemblies.ExportedTypes
                .Where(t => t.Namespace!.Contains("MessageContracts") && t != typeof(andymac4182.Reference.MessageContracts.Constants))
                .MustConformTo(Convention.AllPropertiesMustBeAssignedDuringConstruction())
                .WithFailureAssertion(AssertEx.Fail);
        }
    }
}