using Clientes.Tests.Util;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Clientes.Tests.FunctionalTests
{
    public class ClienteScenarios : ClienteScenarioBase
    {
        [Fact]
        public async Task Cadastrar_Cliente_Response_Created_Status_Code()
        {
            using (var server = CreateServer())
            {
                var content = CreateContent(ClienteSeed.CadastroClienteCommand);

                var response = await server.CreateClient()
                    .PostAsync(ClienteEndPoints.Cadastro, content);

                response.EnsureSuccessStatusCode();
            }
        }
    }
}