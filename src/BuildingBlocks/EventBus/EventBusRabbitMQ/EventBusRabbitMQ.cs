using Autofac;
using EventBus.Core;
using EventBus.Core.Abstractions;
using EventBus.Core.Events;
using EventBus.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        const string BROKER_NAME = "msProject_event_bus";
        private readonly string AUTOFAC_SCOPE_NAME = "msProject_event_bus";
        private IModel _consumerChannel;
        private string _queueName;

        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;
        private readonly IEventBusSubscriptionsManager _eventBusSubscriptionsManager;
        private readonly int _retryCount;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IServiceScopeFactory _serviceScope;
        private readonly ILifetimeScope _lifetimeScope;

        public EventBusRabbitMQ(IRabbitMQPersistentConnection rabbitMQPersistentConnection, ILogger<EventBusRabbitMQ> logger,
            IEventBusSubscriptionsManager eventBusSubscriptionsManager, IServiceScopeFactory serviceScope, ILifetimeScope lifetimeScope, string queueName, int retries = 5)
        {
            _rabbitMQPersistentConnection = rabbitMQPersistentConnection ?? throw new ArgumentNullException(nameof(rabbitMQPersistentConnection));
            _logger = logger;
            _retryCount = retries;
            _queueName = queueName;
            _serviceScope = serviceScope;
            _lifetimeScope = lifetimeScope;
            _eventBusSubscriptionsManager = eventBusSubscriptionsManager ?? new InMemoryEventBusSubscriptionsManager();

            _consumerChannel = CreateConsumerChannel();
            _eventBusSubscriptionsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void StartBasicConsume()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
            {
                _rabbitMQPersistentConnection.TryConnect();
            }

            //_logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _rabbitMQPersistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _eventBusSubscriptionsManager.Clear();
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            await ProcessEvent(eventName, message);

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
            {
                _rabbitMQPersistentConnection.TryConnect();
            }

            using (var channel = _rabbitMQPersistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);

                if (_eventBusSubscriptionsManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
            {
                _rabbitMQPersistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name;

            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using (var channel = _rabbitMQPersistentConnection.CreateModel())
            {

                _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

                channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

                    channel.BasicPublish(
                        exchange: BROKER_NAME,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _eventBusSubscriptionsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            _eventBusSubscriptionsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_rabbitMQPersistentConnection.IsConnected)
                {
                    _rabbitMQPersistentConnection.TryConnect();
                }

                using (var channel = _rabbitMQPersistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: _queueName,
                                      exchange: BROKER_NAME,
                                      routingKey: eventName);
                }
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _eventBusSubscriptionsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _eventBusSubscriptionsManager.RemoveSubscription<T, TH>();
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_eventBusSubscriptionsManager.HasSubscriptionsForEvent(eventName))
            {
                if(_lifetimeScope != null)
                {
                    using (var scope = _lifetimeScope.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                    {
                        var subscriptions = _eventBusSubscriptionsManager.GetHandlersForEvent(eventName);
                        foreach (var subscription in subscriptions)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _eventBusSubscriptionsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            await Task.Yield();
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
                else
                {
                    using (var scope = _serviceScope.CreateScope())
                    {
                        var subscriptions = _eventBusSubscriptionsManager.GetHandlersForEvent(eventName);
                        foreach (var subscription in subscriptions)
                        {
                            var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _eventBusSubscriptionsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            await Task.Yield();
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }                            
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}