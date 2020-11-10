using EventBus.Core.Events;

namespace Clientes.Application.Clientes.IntegrationEvents.Events
{
    public class ClienteCriadoIntegrationEvent : IntegrationEvent
    {
        public long ClienteId { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }

        public ClienteCriadoIntegrationEvent(long clienteId, string cpf, string nome)
        {
            ClienteId = clienteId;
            CPF = cpf;
            Nome = nome;
        }
    }
}