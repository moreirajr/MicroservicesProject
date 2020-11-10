using AutoMapper;
using Clientes.Application.Clientes.Commands;
using Clientes.Application.Clientes.IntegrationEvents.Events;
using Clientes.Application.Clientes.ViewModels;
using Clientes.Domain.Clientes;
using Clientes.Domain.Enums;
using Clientes.Domain.MongoModels;
using Clientes.Domain.Pessoas;
using System;
using System.Threading.Tasks;

namespace Clientes.Application.Clientes
{
    public class ClienteAppService : IClienteAppService
    {
        private readonly IClienteQueryRepository _clienteQueryRepository;
        private readonly IClienteService _clienteService;
        private readonly IClienteIntegrationEventService _clienteIntegrationEventService;
        private readonly IMapper _mapper;

        public ClienteAppService(IClienteService clienteService, IClienteQueryRepository clienteQueryRepository,
            IMapper mapper, IClienteIntegrationEventService clienteIntegrationEventService)
        {
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _clienteQueryRepository = clienteQueryRepository ?? throw new ArgumentNullException(nameof(clienteQueryRepository));
            _clienteIntegrationEventService = clienteIntegrationEventService ?? throw new ArgumentNullException(nameof(clienteIntegrationEventService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ClienteCadastroVM> CadastrarCliente(CadastrarClienteCommand cadastrarClienteCommand)
        {
            var telefone = new Telefone(cadastrarClienteCommand.DDI, cadastrarClienteCommand.DDD, cadastrarClienteCommand.NumeroTelefone, true);

            var endereco = new Endereco(
                cadastrarClienteCommand.CEP,
                cadastrarClienteCommand.Logradouro,
                cadastrarClienteCommand.NumeroEndereco,
                cadastrarClienteCommand.Complemento,
                true);

            var pessoa = new Pessoa(cadastrarClienteCommand.Nome, cadastrarClienteCommand.CPF, cadastrarClienteCommand.DataNascimento, telefone, endereco);
            var cliente = new Cliente(cadastrarClienteCommand.TipoClienteId, EStatusCliente.Ativo.Id, pessoa);

            await _clienteService.CadastrarCliente(cliente);
            _ = _clienteQueryRepository.Adicionar(new ClienteDocument(cliente));

            var integrationEvent = new ClienteCriadoIntegrationEvent(cliente.Id, cliente.Pessoa.CPF, cliente.Pessoa.Nome);
            _ = _clienteIntegrationEventService.PublishEventsThroughEventBusAsync(integrationEvent);

            return _mapper.Map<ClienteCadastroVM>(cliente);
        }

        public async Task<ClienteVM> ConsultarClientePorCpf(string cpf)
        {
            var cliente = await _clienteQueryRepository.ConsultarClientePorCpf(cpf);

            return _mapper.Map<ClienteVM>(cliente);
        }
    }
}