using System;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monolith.Core.Extensions;
using Monolith.Core.Mvc.Filters;
using Monolith.Core.Mvc.Framework;
using Monolith.Core.Mvc.Internal;
using Monolith.Core.Mvc.Options;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Monolith.Core.Mvc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the default Core MVC Services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCoreDefaults(this IServiceCollection services)
        {
            //
            // Libraries

            // DI from "Microsoft.AspNetCore.Mvc".
            services.AddHttpClient();

            // DI from "Microsoft.AspNetCore.Mvc.Versioning".
            services.AddApiVersioning();

            // DI from "AutoMapper".
            services.AddAutoMapper(Dependencies.Assemblies);

            // DI from "FluentValidation.AspNetCore".
            services.AddFluentValidation().AddValidatorsFromAssemblies(Dependencies.Assemblies);

            //
            // Framework

            services.AddScoped<ISession, Session>();

            return services;
        }

        /// <summary>
        /// Add the default Core MVC Controllers.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="setup">An action to configure the provided MVC Options.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCoreControllers(this IServiceCollection services, Action<MvcOptions> setup = null)
        {
            var builder = services.AddControllers(options =>
            {
                options.Filters.Add<FluentValidationActionFilter>();

                setup?.Invoke(options);
            });

            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        /// <summary>
        /// Add the default Core MVC Session.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCoreSession(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.Load<SessionOptions>();

            services.AddSession(session =>
            {
                session.Cookie.Name = options.Cookie;
                session.IdleTimeout = TimeSpan.FromHours(options.Timeout);
            });

            return services;
        }

        /// <summary>
        /// Add the default Core MVC Redis Cache.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCoreRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.Load<RedisOptions>();

            services.AddStackExchangeRedisCache(redis =>
            {
                redis.InstanceName = options.Instance;
                redis.ConfigurationOptions = ConfigurationOptions.Parse(options.Hostname);
            });

            return services;
        }

        /// <summary>
        /// Add the default Core MVC Swagger.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="setup">An action to configure the provided Swagger Options.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCoreSwagger(this IServiceCollection services, Action<SwaggerGenOptions> setup = null)
        {
            services.AddSwaggerGen(options =>
            {
                options.LowercaseDocuments();

                options.AspNetCoreVersioning();

                options.AddSecurity();

                setup?.Invoke(options);
            });

            return services;
        }
    }
}
