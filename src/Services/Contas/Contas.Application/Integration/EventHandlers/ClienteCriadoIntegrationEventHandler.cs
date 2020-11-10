using System;
using System.Threading.Tasks;
using Contas.Application.Integration.Events;
using Contas.Domain.Interfaces;
using Contas.Domain.Models;
using EventBus.Core.Abstractions;

namespace Contas.Application.Integration.EventHandlers
{
    public class ClienteCriadoIntegrationEventHandler : IIntegrationEventHandler<ClienteCriadoIntegrationEvent>
    {
        private readonly IContaCommandRepository _contaCommandRepository;

        public ClienteCriadoIntegrationEventHandler(IContaCommandRepository contaCommandRepository)
        {
            _contaCommandRepository = contaCommandRepository ?? throw new ArgumentNullException(nameof(contaCommandRepository));
        }

        public async Task Handle(ClienteCriadoIntegrationEvent @event)
        {
            var clienteConta = new ClienteConta(@event.ClienteId, @event.CPF, @event.Nome);
            var novaConta = new Conta(clienteConta);

            await _contaCommandRepository.CadastrarNovaConta(novaConta);
        }
    }
}