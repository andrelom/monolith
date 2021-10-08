using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Monolith.Foundation.Messaging.Base
{
    public abstract class ServiceBusConsumerBackgroundService<TMessage> : BackgroundService where TMessage : ServiceBusMessage
    {
        protected abstract Task ConsumeAsync(TMessage message);
    }
}
