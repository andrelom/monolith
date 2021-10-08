using System;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Monolith.Core.Mvc.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use the Core MVC Middlewares.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app)
        {
            return app;
        }

        /// <summary>
        /// Use the Core MVC Swagger.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="setup">An action to configure the provided Swagger Options.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseCoreSwagger(this IApplicationBuilder app, Action<SwaggerUIOptions> setup = null)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;

                options.DefaultModelsExpandDepth(-1);

                setup?.Invoke(options);
            });

            return app;
        }

        /// <summary>
        /// Use the Core MVC Health Checks.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="setup">An action to configure the provided Swagger Options.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseCoreHealthChecks(this IApplicationBuilder app, Action<SwaggerUIOptions> setup = null)
        {
            app.UseHealthChecks("/_/liveness", new HealthCheckOptions
            {
                Predicate = req => req.Name.Contains("self", StringComparison.CurrentCultureIgnoreCase)
            });

            app.UseHealthChecks("/_/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return app;
        }

        /// <summary>
        /// Use the Core MVC Endpoints.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="setup">An action to configure the provided Endpoint Route Builder.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseCoreEndpoints(this IApplicationBuilder app, Action<IEndpointRouteBuilder> setup = null)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                setup?.Invoke(endpoints);
            });

            return app;
        }
    }
}
