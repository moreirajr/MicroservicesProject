using System.Threading.Tasks;
using EventBus.Core.Abstractions;
using EventBus.Core.Events;

namespace Clientes.Application.Clientes
{
    public class ClienteIntegrationEventService : IClienteIntegrationEventService
    {
        private readonly IEventBus _eventBus;

        public ClienteIntegrationEventService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task PublishEventsThroughEventBusAsync(IntegrationEvent integrationEvent)
        {
            _eventBus.Publish(integrationEvent);
        }
    }
}