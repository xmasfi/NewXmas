using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NewShore.Api.Extensions.Configuration
{
    public static class HealthChecksExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("NewShoreConnection"))
                .AddUrlGroup(
                    new Uri("https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration"),
                    "wan",
                    HealthStatus.Degraded)
                .Services;
        }
    }
}