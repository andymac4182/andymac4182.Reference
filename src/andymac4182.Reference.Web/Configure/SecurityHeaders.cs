using Microsoft.AspNetCore.Builder;

namespace andymac4182.Reference.Web.Configure
{
    public static class SecurityHeaders
    {
        public static IApplicationBuilder UseCustomSecurityHeaders(this IApplicationBuilder applicationBuilder)
        {
            var policyCollection = new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders()
                .RemoveServerHeader();

            applicationBuilder.UseSecurityHeaders(policyCollection);

            return applicationBuilder;
        }
    }
}
