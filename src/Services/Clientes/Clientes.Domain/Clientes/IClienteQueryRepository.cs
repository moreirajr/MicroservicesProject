using Clientes.Domain.MongoModels;
using System.Threading.Tasks;

namespace Clientes.Domain.Clientes
{
    public interface IClienteQueryRepository
    {
        Task<ClienteDocument> ConsultarClientePorCpf(string cpf);
        Task Adicionar(ClienteDocument cliente);
    }
}