using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Monolith.Core.Mvc.Extensions;
using Monolith.Foundation.Mail.Extensions;
using Monolith.Foundation.Messaging.Extensions;
using Monolith.Web.Identity.Data.Contexts;
using Monolith.Web.Identity.Data.Entities;
using Monolith.Web.Identity.Healthz;
using Monolith.Web.Identity.Providers;
using Monolith.Web.Identity.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Monolith.Web.Identity
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
            // Libraries

            // DI from "Microsoft.AspNetCore.Identity.EntityFrameworkCore".
            services.AddDbContext<DefaultDbContext>(AddDbContext);

            // DI from "Microsoft.AspNetCore.Identity".
            AddIdentity(services);

            //
            // Core MVC

            services
                .AddCoreDefaults()
                .AddCoreDataProtection(_configuration)
                .AddCoreSession(_configuration)
                .AddCoreRedisCache(_configuration)
                .AddCoreAuthentication(_configuration)
                .AddCoreControllers()
                .AddCoreSwagger(AddSwagger);

            //
            // Foundation

            services
                .AddFoundationMessaging()
                .AddFoundationMail();

            //
            // Health Checks

            services
                .AddHealthChecks()
                .AddCheck<DefaultHealthCheck>("Default");

            //
            // Web

            // Providers

            services.AddScoped<ISecurityKeyProvider, SymmetricSecurityKeyProvider>();
            services.AddScoped<ISecurityTokenProvider, JwtSecurityTokenProvider>();

            // Services

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailSenderService, MailSenderService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCoreMiddlewares();

            app.UseCoreSwagger(UseSwagger);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseCoreEndpoints();

            app.UseCoreHealthChecks();
        }

        #region Private Methods: Add

        private void AddDbContext(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("Default"));
        }

        private void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DefaultDbContext>()
                .AddDefaultTokenProviders();

            // Configure the Identity Options for the identity system.
            services.Configure<IdentityOptions>(options =>
            {
                // Sets the User options.
                options.User.RequireUniqueEmail = true;

                // Sets the SignIn options.
                options.SignIn.RequireConfirmedEmail = true;

                // Sets the Lockout options.
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                // Sets the Password options.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });
        }

        private void AddSwagger(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Monolith.Web.Identity", Version = "v1" });
        }

        #endregion

        #region Private Methods: Use

        private void UseSwagger(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Monolith.Web.Identity v1");
        }

        #endregion
    }
}
