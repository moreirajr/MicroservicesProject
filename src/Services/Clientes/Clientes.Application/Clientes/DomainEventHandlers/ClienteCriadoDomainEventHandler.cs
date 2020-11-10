using Clientes.Application.Clientes.IntegrationEvents.Events;
using Clientes.Domain.DomainEvents;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Clientes.Application.Clientes.DomainEventHandlers
{
    public class ClienteCriadoDomainEventHandler : INotificationHandler<ClienteCriadoDomainEvent>
    {
        private readonly IClienteIntegrationEventService _clienteIntegrationEventService;

        public ClienteCriadoDomainEventHandler(IClienteIntegrationEventService clienteIntegrationEventService)
        {
            _clienteIntegrationEventService = clienteIntegrationEventService ?? throw new ArgumentNullException(nameof(clienteIntegrationEventService));
        }

        public async Task Handle(ClienteCriadoDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new ClienteCriadoIntegrationEvent(notification.Id, notification.CPF, notification.Nome);
            await _clienteIntegrationEventService.PublishEventsThroughEventBusAsync(integrationEvent);
        }
    }
}