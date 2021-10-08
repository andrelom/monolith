using System.Threading.Tasks;
using Monolith.Foundation.Mail.Messages;

namespace Monolith.Foundation.Mail.Framework
{
    public interface IMailClient
    {
        Task SendAsync(MailServiceBusMessage message);
    }
}
