using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monolith.Core.Extensions;
using Monolith.Foundation.Mail.Framework;
using Monolith.Foundation.Mail.Messages;
using Monolith.Foundation.Messaging;
using Monolith.Foundation.Messaging.Options;
using Polly;
using Polly.Retry;

namespace Monolith.Foundation.Mail
{
    public class MailServiceBusConsumerBackgroundService : RabbitMqServiceBusConsumerBackgroundService<MailServiceBusMessage>
    {
        private readonly ILogger<MailServiceBusConsumerBackgroundService> _logger;

        private readonly AsyncRetryPolicy _retry;

        private readonly IMailClient _mailClient;

        public MailServiceBusConsumerBackgroundService(
            IConfiguration configuration,
            RabbitMqConnectionContext context,
            ILogger<MailServiceBusConsumerBackgroundService> logger,
            IMailClient mailClient) : base(
            configuration.Load<RabbitMqModelOptions>(),
            logger,
            context)
        {
            _logger = logger;
            _retry = CreateRetryPolicy();
            _mailClient = mailClient;
        }

        protected override async Task ConsumeAsync(MailServiceBusMessage mail)
        {
            await _retry.ExecuteAsync(async () =>
                await _mailClient.SendAsync(mail).ConfigureAwait(false)).ConfigureAwait(false);
        }

        #region Private Methods

        private AsyncRetryPolicy CreateRetryPolicy()
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(3, attempt =>
            {
                var wait = TimeSpan.FromSeconds(Math.Pow(2, attempt));

                _logger.LogWarning($"Mail Service Bus Consumer: Waiting {wait.TotalSeconds} seconds");

                return wait;
            });
        }

        #endregion
    }
}
