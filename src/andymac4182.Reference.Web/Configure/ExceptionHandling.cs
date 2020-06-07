using andymac4182.Reference.Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;

namespace andymac4182.Reference.Web.Configure
{
    public static class ExceptionHandling
    {
        public static IApplicationBuilder UseCustomExceptionHandling(
            this IApplicationBuilder applicationBuilder,
            EnvironmentTypeSetting environmentType)
            =>
                environmentType.IsLocal()
                    ? applicationBuilder
                        .UseDeveloperExceptionPage()
                    : applicationBuilder
                        .UseExceptionHandler("/error")
                        .UseStatusCodePagesWithReExecute("/error/{0}");
    }
}