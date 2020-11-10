using Clientes.Application.Clientes;
using Clientes.Application.Mapper;
using Clientes.Domain.Clientes;
using Clientes.Infra.Data.CommandRepositories;
using Clientes.Infra.Data.EF;
using Clientes.Infra.Data.Mongo;
using Clientes.Infra.Data.QueryRepositories;
using EventBus.Core;
using EventBus.Core.Abstractions;
using EventBus.RabbitMQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Clientes.Infra.CrossCutting.IoC
{
    public static class ClienteApplicationModule
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
        {
            var mapperConfig = new AutoMapperConfig();
            services.AddSingleton(mapperConfig.Mapper);

            services.AddMediatR(AppDomain.CurrentDomain.Load("Clientes.Application"));

            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddSingleton<IApplicationContextDbNoSql, ApplicationContextDbNoSql>(x =>
                new ApplicationContextDbNoSql(
                    x.GetService<IMongoDbSettings>().ConnectionString,
                    x.GetService<IMongoDbSettings>().DatabaseName));

            services.AddScoped<IClienteQueryRepository, ClienteQueryRepository>();

            services.AddScoped<IClienteCommandRepository, ClienteCommandRepository>();

            services.AddScoped<IClienteService, ClienteService>();

            services.AddScoped<IClienteAppService, ClienteAppService>();

            return services;
        }

        public static IServiceCollection ConfigureEF(this IServiceCollection services, string connectionString)
        {
            services.InitializeEFConfiguration(connectionString);

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var rabbitMQConfiguration = sp.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;

                var connectionFactoryConfig = new RabbitMQConnectionFactoryConfiguration(
                    rabbitMQConfiguration.EventBusConnection,
                    rabbitMQConfiguration.EventBusUserName,
                    rabbitMQConfiguration.EventBusPassword);

                var retryCount = 5;
                if (!string.IsNullOrEmpty(rabbitMQConfiguration.EventBusRetryCount))
                {
                    retryCount = int.Parse(rabbitMQConfiguration.EventBusRetryCount);
                }

                return new EventBusRabbitMQ(
                    rabbitMQPersistentConnection,
                    logger,
                    eventBusSubcriptionsManager,
                    serviceScopeFactory,
                    null,
                    rabbitMQConfiguration.SubscriptionClientName,
                    retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }

        public static IServiceCollection AddIntegrations(this IServiceCollection services)
        {
            services.AddTransient<IClienteIntegrationEventService, ClienteIntegrationEventService>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

                var rabbitMQConfiguration = sp.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;

                var connectionFactoryConfig = new RabbitMQConnectionFactoryConfiguration(
                    rabbitMQConfiguration.EventBusConnection,
                    rabbitMQConfiguration.EventBusUserName,
                    rabbitMQConfiguration.EventBusPassword);

                return new RabbitMQPersistentConnection(connectionFactoryConfig, logger);
            });

            return services;
        }

        public static void ConfigureEventBus(this IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            //eventBus.Subscribe<T, IIntegrationEventHandler<THandler>>();
        }
    }
}