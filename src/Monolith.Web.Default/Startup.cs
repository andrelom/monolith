using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Monolith.Core.Mvc.Extensions;
using Monolith.Foundation.Identity.Extensions;
using Monolith.Web.Default.Healthz;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Monolith.Web.Default
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //
            // Core MVC

            services
                .AddCoreDefaults()
                .AddCoreRedisCache(_configuration)
                .AddCoreControllers()
                .AddCoreSwagger(AddSwagger);

            //
            // Foundation

            services
                .AddFoundationIdentity(_configuration);

            //
            // Health Checks

            services
                .AddHealthChecks()
                .AddCheck<DefaultHealthCheck>("Default");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCoreMiddlewares();

            app.UseCoreSwagger(UseSwagger);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseFoundationIdentity();

            app.UseCoreEndpoints();

            app.UseCoreHealthChecks();
        }

        #region Private Methods: Add

        private void AddSwagger(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Monolith.Web.Default", Version = "v1" });
        }

        #endregion

        #region Private Methods: Use

        private void UseSwagger(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Monolith.Web.Default v1");
        }

        #endregion
    }
}
