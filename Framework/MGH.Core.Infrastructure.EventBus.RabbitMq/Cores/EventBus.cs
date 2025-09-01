﻿using System.Text;
using System.Text.Json;
using MGH.Core.Domain.Events;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Attributes;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Configurations;
using MGH.Core.Infrastructure.EventBus.RabbitMq.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MGH.Core.Infrastructure.EventBus.RabbitMq.Cores;

public class EventBus : IEventBus
{
    private readonly IRabbitConnection _rabbitConnection;
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMqOptions _rabbitMqOptions;

    public EventBus(
        IRabbitConnection rabbitConnection,
        IServiceProvider serviceProvider,
        IOptions<RabbitMqOptions> options)
    {
        _rabbitConnection = rabbitConnection;
        _serviceProvider = serviceProvider;
        _rabbitConnection.ConnectService();
        _rabbitMqOptions = options.Value;
        BindExchangesAndQueues(_rabbitConnection.GetChannel());
    }

    public void Publish<T>(T model) where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

        var basicProperties = channel.CreateBasicProperties();
        var messageJson = JsonSerializer.Serialize(model);
        var messageByte = Encoding.UTF8.GetBytes(messageJson);

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));

        channel.QueueBind(
           queue: _rabbitMqOptions.EventBus.QueueName,
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           routingKey: baseMessage.RoutingKey);

        channel.BasicPublish(
            exchange: _rabbitMqOptions.EventBus.ExchangeName,
            routingKey: baseMessage.RoutingKey,
            basicProperties: basicProperties,
            body: messageByte);
    }

    public void Publish<T>(IEnumerable<T> models) where T : IEvent
    {
        if (models == null || !models.Any())
            throw new ArgumentException("The collection of models cannot be null or empty.", nameof(models));

        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));

        channel.QueueBind(
           queue: _rabbitMqOptions.EventBus.QueueName,
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           routingKey: baseMessage.RoutingKey);

        var basicProperties = channel.CreateBasicProperties();
        foreach (var model in models)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageByte = Encoding.UTF8.GetBytes(messageJson);

            channel.BasicPublish(
                exchange: _rabbitMqOptions.EventBus.ExchangeName,
                routingKey: baseMessage.RoutingKey,
                basicProperties: basicProperties,
                body: messageByte);
        }
    }

    public void Consume<T>(Func<T, Task> handler) where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));
        channel.QueueBind(
           queue: _rabbitMqOptions.EventBus.QueueName,
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           routingKey: baseMessage.RoutingKey);

        var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(json);
                if (message != null)
                    await handler(message);

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        channel.BasicConsume(_rabbitMqOptions.EventBus.QueueName, false, consumer);
    }

    public void Consume<T>() where T : IEvent
    {
        _rabbitConnection.ConnectService();
        var channel = _rabbitConnection.GetChannel();

        var baseMessage = GetBaseMessageFromAttribute(typeof(T));

        channel.QueueBind(
           queue: _rabbitMqOptions.EventBus.QueueName,
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           routingKey: baseMessage.RoutingKey);

        var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            using var scope = _serviceProvider.CreateScope();
            try
            {
                var handler = scope.ServiceProvider.GetService<IEventHandler<T>>();
                if (handler == null)
                    throw new InvalidOperationException($"Handler for event type {typeof(T).Name} not registered.");

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (message == null)
                {
                    channel.BasicNack(ea.DeliveryTag, false, false); // discard message
                    return;
                }

                await handler.HandleAsync(message);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Error handling message for type {typeof(T).Name}: {ex.Message}");
                Console.WriteLine($"Raw message: {json}");

                channel.BasicNack(ea.DeliveryTag, false, false); // reject and don't requeue
            }
        };

        channel.BasicConsume(queue: _rabbitMqOptions.EventBus.QueueName, autoAck: false, consumer: consumer);
    }

    private BaseMessage GetBaseMessageFromAttribute(Type type)
    {
        var attribute = type
            .GetCustomAttributes(typeof(EventRoutingAttribute), true)
            .FirstOrDefault() as EventRoutingAttribute;

        if (attribute == null)
            throw new InvalidOperationException($"EventRoutingAttribute is not defined for type {type.Name}.");

        return new BaseMessage(attribute.RoutingKey);
    }

    public void BindExchangesAndQueues(IModel channel)
    {
        channel.ExchangeDeclare(
           exchange: _rabbitMqOptions.EventBus.ExchangeName,
           type: _rabbitMqOptions.EventBus.ExchangeType,
           durable: true,
           autoDelete: false,
           arguments: null);

        channel.QueueDeclare(
            queue: _rabbitMqOptions.EventBus.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);


        foreach (var item in _rabbitMqOptions.EventBus.EndToEndExchangeBindings)
        {
            if (string.IsNullOrWhiteSpace(item.SourceExchange) ||
                string.IsNullOrWhiteSpace(item.DestinationExchange) ||
                string.IsNullOrWhiteSpace(item.RoutingKey))
                continue;

            channel.ExchangeDeclare(
                exchange: item.SourceExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            channel.ExchangeDeclare(
                exchange: item.DestinationExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            channel.ExchangeBind(
                destination: item.DestinationExchange,
                source: item.SourceExchange,
                routingKey: item.RoutingKey);

            channel.QueueDeclare(
                queue: _rabbitMqOptions.EventBus.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(
                queue: _rabbitMqOptions.EventBus.QueueName,
                exchange: item.DestinationExchange,
                routingKey: item.RoutingKey);
        }
    }
}
