using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace AuthenticationService.Infrastructure.Messaging
{
    public class RabbitMQConnection : IDisposable
    {
        private readonly IConfiguration _config;
        private readonly ILogger<RabbitMQConnection> _logger;
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;

        public RabbitMQConnection(IConfiguration config, ILogger<RabbitMQConnection> logger)
        {
            _config = config;
            _logger = logger;

            _factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"],
                UserName = _config["RabbitMQ:Username"],
                Password = _config["RabbitMQ:Password"]
            };
        }

        /// <summary>
        /// Gets the RabbitMQ connection, creating it if necessary.
        /// </summary>
        public IConnection GetConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _logger.LogInformation("Establishing a new RabbitMQ connection...");
                _connection = _factory.CreateConnection();
            }

            return _connection;
        }

        /// <summary>
        /// Closes the connection when the application shuts down.
        /// </summary>
        public void Dispose()
        {
            if (_connection != null && _connection.IsOpen)
            {
                _logger.LogInformation("Closing RabbitMQ connection...");
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}
