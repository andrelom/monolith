using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Monolith.Foundation.Messaging.Base;
using Monolith.Foundation.Messaging.Options;
using RabbitMQ.Client;

namespace Monolith.Foundation.Messaging
{
    public class RabbitMqServiceBusPublisher<TMessage> : IServiceBusPublisher<TMessage> where TMessage : ServiceBusMessage
    {
        private bool _disposed;

        private readonly RabbitMqModelOptions _options;

        private readonly IModel _model;

        public RabbitMqServiceBusPublisher(
            RabbitMqConnectionContext context,
            RabbitMqModelOptions options)
        {
            _options = options;
            _model = context.CreateModel();

            _model.ExchangeDeclare(_options.Exchange, ExchangeType.Topic);
            _model.QueueDeclare(_options.Queue, false, false, false, null);
            _model.QueueBind(_options.Queue, _options.Exchange, _options.RoutingKey, null);
            _model.BasicQos(0, 1, false);
        }

        public Task PublishAsync(TMessage message)
        {
            var payload = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(payload);

            _model.BasicPublish(_options.Exchange, _options.RoutingKey, null, body);

            return Task.CompletedTask;
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _model.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
