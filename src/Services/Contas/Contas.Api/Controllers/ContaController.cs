using Contas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Contas.Api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly IContaAppService _contaAppService;

        public ContaController(IContaAppService contaAppService)
        {
            _contaAppService = contaAppService ?? throw new ArgumentNullException(nameof(contaAppService));
        }

        [HttpGet]
        public async Task<IActionResult> ObterContaPorCPF(string cpf)
        {
            var contaCliente = await _contaAppService.ObterContaPorCPF(cpf);

            if (contaCliente == null) return NotFound("Cliente não encontrado.");

            return Ok(contaCliente);
        }

        [HttpPost]
        [Route("solicitar-cartao")]
        public async Task<IActionResult> SolicitarCartao(long contaId)
        {
            var result = await _contaAppService.SolicitarCartao(contaId);

            if (result == null)
                return BadRequest("Erro ao solicitar cartão.");

            return Ok(result);
        }
    }
}