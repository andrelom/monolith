using Monolith.Core.Attributes;

namespace Monolith.Foundation.Messaging.Options
{
    [Option("Foundation:Mail:RabbitMq:Sender")]
    public class RabbitMqModelOptions
    {
        public string Exchange { get; set; } = "monolith.exchange";

        public string RoutingKey { get; set; } = "monolith.queue.*";

        public string Queue { get; set; } = "monolith.queue.default";
    }
}
