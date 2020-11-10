using Clientes.Application.Clientes.Commands;
using Clientes.Application.Clientes.ViewModels;
using System.Threading.Tasks;

namespace Clientes.Application.Clientes
{
    public interface IClienteAppService
    {
        Task<ClienteCadastroVM> CadastrarCliente(CadastrarClienteCommand cadastrarClienteCommand);
        Task<ClienteVM> ConsultarClientePorCpf(string cpf);
    }
}