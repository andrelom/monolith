using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monolith.Core.Extensions;
using Monolith.Foundation.Mail.Messages;
using Monolith.Foundation.Mail.Options;

namespace Monolith.Foundation.Mail.Framework
{
    public class MailClient : IMailClient
    {
        private readonly MailOptions _options;

        private readonly ILogger<MailClient> _logger;

        public MailClient(
            IConfiguration configuration,
            ILogger<MailClient> logger)
        {
            _options = configuration.Load<MailOptions>();
            _logger = logger;
        }

        public Task SendAsync(MailServiceBusMessage message)
        {
            var path = Path.Join(_options.Path, message.View);

            if (!File.Exists(path))
            {
                _logger.LogError($"Basic Mail Client: View '{message.View}' not found");

                return Task.CompletedTask;
            }

            var template = File.ReadAllText(path);

            _logger.LogInformation(template.Template(message.Data));

            return Task.CompletedTask;
        }
    }
}
