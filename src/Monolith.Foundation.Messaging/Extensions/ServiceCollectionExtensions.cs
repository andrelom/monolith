using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monolith.Core.Extensions;
using Monolith.Foundation.Messaging.Options;

namespace Monolith.Foundation.Messaging.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFoundationMessaging(this IServiceCollection services)
        {
            // Singletons
            services.AddSingleton(RabbitMqContextFactory);

            return services;
        }

        #region Private Methods

        private static RabbitMqConnectionContext RabbitMqContextFactory(IServiceProvider provider)
        {
            var configuration = provider.GetService<IConfiguration>();
            var options = configuration.Load<RabbitMqConnectionOptions>();

            return new RabbitMqConnectionContext(options);
        }

        #endregion
    }
}
