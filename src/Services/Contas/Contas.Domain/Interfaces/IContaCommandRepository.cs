using Contas.Domain.Models;
using System.Threading.Tasks;

namespace Contas.Domain.Interfaces
{
    public interface IContaCommandRepository
    {
        Task<Conta> CadastrarNovaConta(Conta conta);
        Task<Cartao> SolicitarCartao(long contaId);
    }
}