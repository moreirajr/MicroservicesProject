using Clientes.Api.Controllers;
using Clientes.Application.Clientes;
using Clientes.Tests.Util;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Clientes.Tests.UnitTests.Application
{
    public class ClienteWebApiTest
    {
        private readonly Mock<IClienteAppService> _clienteAppServiceMock;

        public ClienteWebApiTest()
        {
            _clienteAppServiceMock = new Mock<IClienteAppService>();
        }

        [Fact]
        public async Task CadastrarCliente_Sucess()
        {
            var clienteCommand = ClienteSeed.CadastroClienteCommand;

            var clienteController = new ClienteController(_clienteAppServiceMock.Object);

            var actionResult = await clienteController.CadastrarCliente(clienteCommand) as CreatedResult;

            Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.Created);
        }
    }
}