using Conventional;

namespace andymac4182.Reference.Tests.Conventions
{
    public class DatabaseConventions
    {
        public void SqlMustHaveMatchingEmbeddedResources()
        {
            DbUp.Constants.DbUpAssembly
                .GetExportedTypes()
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResources("sql"))
                .WithFailureAssertion(AssertEx.Fail);
        }
    }
}