using Contas.Domain.Interfaces;
using Contas.Domain.Models;
using Contas.Infra.Data.Dapper;
using Dapper;
using System.Threading.Tasks;

namespace Contas.Infra.Data.Repositories
{
    public class ContaCommandRepository : IContaCommandRepository
    {
        private readonly ContasContextDb _contasContextDb;
        private string InserirCliente => $@"INSERT INTO [dbo].[Cliente](Id, CPF, Nome)
        OUTPUT INSERTED.* VALUES (@{nameof(ClienteConta.Id)}, @{nameof(ClienteConta.CPF)}, @{nameof(ClienteConta.Nome)})";

        private string CriarNovaConta => "INSERT INTO [dbo].[Conta](ClienteId, Saldo) OUTPUT INSERTED.* VALUES (@clienteId, 0)";
        private string CriarNovoCartao => $@"INSERT INTO [dbo].[Cartao](ContaId, Numero, Codigo, Bloqueado)
        OUTPUT INSERTED.* VALUES (@{nameof(Cartao.ContaId)}, @{nameof(Cartao.Numero)}, @{nameof(Cartao.Codigo)}, @{nameof(Cartao.Bloqueado)})";

        private string BuscarContaPorId => "SELECT * FROM [dbo].[Conta] WHERE [Id] = @Id";

        public ContaCommandRepository(ContasContextDb contasContextDb)
        {
            _contasContextDb = contasContextDb;
        }

        public async Task<Conta> CadastrarNovaConta(Conta conta)
        {
            Conta result = null;
            try
            {
                var resultCliente = await _contasContextDb.Connection.QueryFirstAsync<ClienteConta>(InserirCliente, conta.Cliente);
                result = await _contasContextDb.Connection.QueryFirstAsync<Conta>(CriarNovaConta, new { @clienteId = resultCliente.Id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<Cartao> SolicitarCartao(long contaId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Id", contaId);

                var conta = await _contasContextDb.Connection.QueryFirstOrDefaultAsync<Conta>(BuscarContaPorId, param);
                if (conta == null) return null;

                var cartao = new Cartao(contaId);
                return await _contasContextDb.Connection.QueryFirstAsync<Cartao>(CriarNovoCartao, cartao);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}