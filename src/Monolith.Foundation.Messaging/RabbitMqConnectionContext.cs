using System;
using Monolith.Foundation.Messaging.Options;
using RabbitMQ.Client;

namespace Monolith.Foundation.Messaging
{
    public class RabbitMqConnectionContext : IDisposable
    {
        private bool _disposed;

        private readonly IConnection _connection;

        public RabbitMqConnectionContext(RabbitMqConnectionOptions options)
        {
            var factory = new ConnectionFactory
            {
                HostName = options.Hostname,
                UserName = options.Username,
                Password = options.Password
            };

            _connection = factory.CreateConnection();
        }

        public IModel CreateModel()
        {
            return _connection.CreateModel();
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
                _connection.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
