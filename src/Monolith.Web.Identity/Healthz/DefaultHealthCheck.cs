using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Monolith.Web.Identity.Healthz
{
    public class DefaultHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token)
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
