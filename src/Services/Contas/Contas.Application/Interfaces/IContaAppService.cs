using Contas.Application.ViewModels;
using System.Threading.Tasks;

namespace Contas.Application.Interfaces
{
    public interface IContaAppService //: IDisposable
    {
        Task<ContaClienteVM> ObterContaPorCPF(string cpf);
        Task<CartaoVM> SolicitarCartao(long contaId);
    }
}