using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Monolith.Foundation.Messaging.Base;
using Monolith.Foundation.Messaging.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Monolith.Foundation.Messaging
{
    public abstract class RabbitMqServiceBusConsumerBackgroundService<TMessage> : ServiceBusConsumerBackgroundService<TMessage> where TMessage : ServiceBusMessage
    {
        private readonly RabbitMqModelOptions _options;

        private readonly ILogger<RabbitMqServiceBusConsumerBackgroundService<TMessage>> _logger;

        private readonly IModel _model;

        protected RabbitMqServiceBusConsumerBackgroundService(
            RabbitMqModelOptions options,
            ILogger<RabbitMqServiceBusConsumerBackgroundService<TMessage>> logger,
            RabbitMqConnectionContext context)
        {
            _options = options;
            _logger = logger;
            _model = context.CreateModel();

            _model.ExchangeDeclare(_options.Exchange, ExchangeType.Topic);
            _model.QueueDeclare(_options.Queue, false, false, false, null);
            _model.QueueBind(_options.Queue, _options.Exchange, _options.RoutingKey, null);
            _model.BasicQos(0, 1, false);
        }

        protected override Task ExecuteAsync(CancellationToken token)
        {
            var consumer = new AsyncEventingBasicConsumer(_model);

            consumer.Received += async (_, args) =>
            {
                if (!token.IsCancellationRequested)
                {
                    await ConsumeAsync(args).ConfigureAwait(false);
                }
            };

            _model.BasicConsume(_options.Queue, false, consumer);

            return Task.CompletedTask;
        }

        #region Dispose

        public override void Dispose()
        {
            _model.Dispose();

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        private async Task ConsumeAsync(BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var payload = Encoding.UTF8.GetString(body);

            try
            {
                await ConsumeAsync(JsonSerializer.Deserialize<TMessage>(payload)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ Consumer", payload);
            }
            finally
            {
                _model.BasicAck(args.DeliveryTag, false);
            }
        }

        #endregion
    }
}
