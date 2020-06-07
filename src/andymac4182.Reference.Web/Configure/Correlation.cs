using CorrelationId.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace andymac4182.Reference.Web.Configure
{
    public static class Correlation
    {
        public static IServiceCollection AddCustomCorrelationId(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddCorrelationId(options =>
                {
                    options.UpdateTraceIdentifier = true;
                })
                .WithTraceIdentifierProvider();

            return serviceCollection;
        }
    }
}