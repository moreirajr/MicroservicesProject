using Clientes.Domain.Clientes;
using Clientes.Domain.MongoModels;
using Clientes.Infra.Data.Mongo;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Clientes.Infra.Data.QueryRepositories
{
    public class ClienteQueryRepository : AQueryRepository, IClienteQueryRepository
    {
        private readonly IMongoCollection<ClienteDocument> _clientesCollection;

        public ClienteQueryRepository(IApplicationContextDbNoSql context) : base(context)
        {
            _clientesCollection = Context.DataBase.GetCollection<ClienteDocument>("Clientes");
            //CreateCollectionsIfNotExists();
        }

        public async Task<ClienteDocument> ConsultarClientePorCpf(string cpf)
        {
            var clienteCursor = await _clientesCollection.FindAsync(x => x.CPF == cpf);

            return clienteCursor.FirstOrDefault();
        }

        public async Task Adicionar(ClienteDocument cliente)
        {
            try
            {
                await _clientesCollection.InsertOneAsync(cliente);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}