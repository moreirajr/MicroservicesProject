using System;
using System.Threading.Tasks;

namespace Clientes.Domain.Clientes
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteCommandRepository _clienteCommandRepository;
        public ClienteService(IClienteCommandRepository clienteCommandRepository)
        {
            _clienteCommandRepository = clienteCommandRepository ?? throw new ArgumentNullException(nameof(clienteCommandRepository));
        }

        public async Task<Cliente> CadastrarCliente(Cliente cliente)
        {
            await _clienteCommandRepository.CadastrarClienteAsync(cliente);

            await _clienteCommandRepository.UnitOfWork.SaveEntitiesAsync();

            return cliente;
        }
    }
}