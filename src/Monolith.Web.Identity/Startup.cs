using System;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Monolith.Core.Extensions;
using Monolith.Core.Mvc.Extensions;
using Monolith.Foundation.Mail.Extensions;
using Monolith.Foundation.Messaging.Extensions;
using Monolith.Web.Identity.Data.Contexts;
using Monolith.Web.Identity.Data.Entities;
using Monolith.Web.Identity.Healthz;
using Monolith.Web.Identity.Options;
using Monolith.Web.Identity.Providers;
using Monolith.Web.Identity.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Monolith.Web.Identity
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private readonly JwtOptions _jwtOptions;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtOptions = configuration.Load<JwtOptions>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //
            // Libraries

            // DI from "Microsoft.AspNetCore.Identity.EntityFrameworkCore".
            services.AddDbContext<DefaultDbContext>(AddDbContext);

            // DI from "Microsoft.AspNetCore.Identity".
            AddIdentity(services);

            // DI from "Microsoft.AspNetCore.Identity".
            services
                .AddAuthentication(AddAuthentication)
                .AddJwtBearer(AddJwtBearer);

            //
            // Core MVC

            services
                .AddCoreDefaults()
                .AddCoreDataProtection(_configuration)
                .AddCoreSession(_configuration)
                .AddCoreRedisCache(_configuration)
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

        private void AddAuthentication(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        private void AddJwtBearer(JwtBearerOptions options)
        {
            // In distributed applications, it should be false.
            options.SaveToken = false;

            // Parameters used to validate the identity token.
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.IssuerKey))
            };
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
