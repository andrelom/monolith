using Monolith.Core.Attributes;

namespace Monolith.Foundation.Messaging.Options
{
    [Option("Foundation:Messaging:RabbitMq")]
    public class RabbitMqConnectionOptions
    {
        public string Hostname { get; set; } = "localhost";

        public string Username { get; set; } = "guest";

        public string Password { get; set; } = "guest";
    }
}
