using System.Linq;
using andymac4182.Reference.Web.Infrastructure.Logger;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Context;

namespace andymac4182.Reference.Web.Configure
{
    public static class Serilog
    {
        public static IApplicationBuilder UseCustomSerilog(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (context, next) =>
            {
                using (LogContext.PushProperty("ClientIP",  context.Connection.RemoteIpAddress.ToString()))
                using (LogContext.PushProperty("UserAgent",context.Request.Headers["User-Agent"].FirstOrDefault()))
                using (LogContext.PushProperty("Resource", context.GetMetricsCurrentResourceName()))
                {
                    await next.Invoke();
                }

                context.GetMetricsCurrentResourceName();
            });
            applicationBuilder.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = LogEnricher.Enrich;
            });
                
            return applicationBuilder;
        }
    }
}