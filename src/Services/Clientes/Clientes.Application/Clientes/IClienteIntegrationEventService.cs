using EventBus.Core.Events;
using System.Threading.Tasks;

namespace Clientes.Application.Clientes
{
    public interface IClienteIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(IntegrationEvent integrationEvent);
        //Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}