using Microsoft.AspNetCore.Builder;

namespace Monolith.Foundation.Identity.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFoundationIdentity(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}
