using Microsoft.Extensions.DependencyInjection;
using BEZNgCore.HealthChecks;

namespace BEZNgCore.Web.HealthCheck;

public static class AbpZeroHealthCheck
{
    public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
    {
        var builder = services.AddHealthChecks();
        builder.AddCheck<BEZNgCoreDbContextHealthCheck>("Database Connection");
        builder.AddCheck<BEZNgCoreDbContextUsersHealthCheck>("Database Connection with user check");
        builder.AddCheck<CacheHealthCheck>("Cache");

        // add your custom health checks here
        // builder.AddCheck<MyCustomHealthCheck>("my health check");

        return builder;
    }
}

