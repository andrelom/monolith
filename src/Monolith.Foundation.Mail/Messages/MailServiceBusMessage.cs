using System.Collections.Generic;
using Monolith.Foundation.Messaging.Base;

namespace Monolith.Foundation.Mail.Messages
{
    public class MailServiceBusMessage : ServiceBusMessage
    {
        public string From { get; set; }

        public string To { get; set; }

        public string View { get; set; }

        public IDictionary<string, string> Data { get; set; }
    }
}
