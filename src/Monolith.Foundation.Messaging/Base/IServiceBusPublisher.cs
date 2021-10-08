using System;
using System.Threading.Tasks;

namespace Monolith.Foundation.Messaging.Base
{
    public interface IServiceBusPublisher<in TMessage> : IDisposable where TMessage : ServiceBusMessage
    {
        Task PublishAsync(TMessage message);
    }
}
