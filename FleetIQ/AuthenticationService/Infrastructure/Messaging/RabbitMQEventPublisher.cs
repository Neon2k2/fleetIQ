using System;
using AuthenticationService.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;

namespace AuthenticationService.Infrastructure.Messaging;

public class RabbitMQEventPublisher : IEventPublisher
{
    private readonly IConfiguration _config;
    private readonly ILogger<RabbitMQEventPublisher> _logger;
    private readonly IModel _channel;

    public RabbitMQEventPublisher(RabbitMQConnection rabbitMQConnection, ILogger<RabbitMQEventPublisher> logger)
    {
        _logger = logger;

        var connection = rabbitMQConnection.GetConnection();
        _channel = connection.CreateModel();

        // Declare exchange
        _channel.ExchangeDeclare("user_events", ExchangeType.Topic);
    }
}