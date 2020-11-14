using Clientes.Application.Clientes;
using Clientes.Application.Clientes.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clientes.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteAppService _clienteAppService;
        public ClienteController(IClienteAppService clienteAppService)
        {
            _clienteAppService = clienteAppService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CadastrarCliente([FromBody] CadastrarClienteCommand command)
        {
            var cliente = await _clienteAppService.CadastrarCliente(command);

            if (cliente == null) return BadRequest("Erro ao cadastrar cliente.");

            return Created(string.Empty, cliente);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConsultarClientePorCpf(string cpf)
        {
            var cliente = await _clienteAppService.ConsultarClientePorCpf(cpf);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            return Ok(cliente);
        }
    }
}