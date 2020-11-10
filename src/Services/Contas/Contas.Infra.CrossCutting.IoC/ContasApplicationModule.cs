using Autofac;
using Autofac.Extensions.DependencyInjection;
using Contas.Application.AutoMapper;
using Contas.Application.CQRS.Commands;
using Contas.Application.Integration.EventHandlers;
using Contas.Application.Integration.Events;
using Contas.Application.Interfaces;
using Contas.Application.Services;
using Contas.Domain.Interfaces;
using Contas.Infra.Data.Dapper;
using Contas.Infra.Data.Repositories;
using EventBus.Core;
using EventBus.Core.Abstractions;
using EventBus.RabbitMQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

namespace Contas.Infra.CrossCutting.IoC
{
    public static class ContasApplicationModule
    {
        public static IServiceProvider ConfigureDependencies(this IServiceCollection services, string connectionString)
        {
            var mapperConfig = new MapperConfig();
            services.AddSingleton(mapperConfig.Mapper);

            services.ConfigureDapper(connectionString);

            services.AddMediatR(AppDomain.CurrentDomain.Load("Contas.Application"));

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterAssemblyTypes(typeof(AtivarCartaoCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            container.RegisterAssemblyTypes(typeof(ClienteCriadoIntegrationEventHandler).GetTypeInfo().Assembly)
             .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

            container.RegisterType<ContaAppService>()
                .As<IContaAppService>()
                .InstancePerLifetimeScope();

            container.RegisterType<ContaQueryRepository>()
                .As<IContaQueryRepository>()
                .InstancePerLifetimeScope();

            container.RegisterType<ContaCommandRepository>()
                .As<IContaCommandRepository>()
                .InstancePerLifetimeScope();

            return new AutofacServiceProvider(container.Build());
        }

        public static IServiceCollection ConfigureDapper(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<ContasContextDb>(x => new ContasContextDb(connectionString));
            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();

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
                    iLifetimeScope,
                    rabbitMQConfiguration.SubscriptionClientName,
                    retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }

        public static IServiceCollection AddIntegrations(this IServiceCollection services)
        {
            services.AddTransient<IContaIntegrationEventService, ContaIntegrationEventService>();

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

            eventBus.Subscribe<ClienteCriadoIntegrationEvent, IIntegrationEventHandler<ClienteCriadoIntegrationEvent>>();
        }
    }
}