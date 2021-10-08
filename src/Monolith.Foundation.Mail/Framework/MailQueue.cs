using System.Collections.Generic;
using System.Threading.Tasks;
using Monolith.Foundation.Mail.Messages;
using Monolith.Foundation.Messaging.Base;

namespace Monolith.Foundation.Mail.Framework
{
    public class MailQueue : IMailQueue
    {
        private readonly IServiceBusPublisher<MailServiceBusMessage> _bus;

        public MailQueue(IServiceBusPublisher<MailServiceBusMessage> bus)
        {
            _bus = bus;
        }

        public async Task EnqueueAsync(string from, string to, string view, IDictionary<string, string> data)
        {
            await _bus.PublishAsync(new MailServiceBusMessage
            {
                From = from,
                To = to,
                View = view,
                Data = data
            }).ConfigureAwait(false);
        }
    }
}
