using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monolith.Core.Extensions;
using Monolith.Foundation.Mail.Framework;
using Monolith.Foundation.Mail.Messages;
using Monolith.Foundation.Messaging;
using Monolith.Foundation.Messaging.Base;
using Monolith.Foundation.Messaging.Options;

namespace Monolith.Foundation.Mail.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFoundationMail(this IServiceCollection services)
        {
            // Singletons
            services.AddSingleton<IServiceBusPublisher<MailServiceBusMessage>, RabbitMqServiceBusPublisher<MailServiceBusMessage>>(MailRabbitMqServiceBusPublisherFactory);

            // Transient
            services.AddTransient<IMailClient, MailClient>();

            // Scoped
            services.AddScoped<IMailQueue, MailQueue>();

            return services;
        }

        public static IServiceCollection AddFoundationMailHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<MailServiceBusConsumerBackgroundService>();

            return services;
        }

        #region Private Methods

        private static RabbitMqServiceBusPublisher<MailServiceBusMessage> MailRabbitMqServiceBusPublisherFactory(IServiceProvider provider)
        {
            var configuration = provider.GetService<IConfiguration>();
            var context = provider.GetService<RabbitMqConnectionContext>();
            var options = configuration.Load<RabbitMqModelOptions>();

            return new RabbitMqServiceBusPublisher<MailServiceBusMessage>(context, options);
        }

        #endregion
    }
}
