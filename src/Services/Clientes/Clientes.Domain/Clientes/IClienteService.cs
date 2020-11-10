using System.Threading.Tasks;

namespace Clientes.Domain.Clientes
{
    public interface IClienteService
    {
        Task<Cliente> CadastrarCliente(Cliente cliente);
    }
}