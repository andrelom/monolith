using System;

namespace Monolith.Foundation.Messaging.Base
{
    public abstract class ServiceBusMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
