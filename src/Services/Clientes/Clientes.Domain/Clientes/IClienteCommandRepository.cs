using Clientes.Domain.SeedWork;
using System.Threading.Tasks;

namespace Clientes.Domain.Clientes
{
    public interface IClienteCommandRepository : IRepository<Cliente>
    {
        Task<Cliente> CadastrarClienteAsync(Cliente cliente);
    }
}