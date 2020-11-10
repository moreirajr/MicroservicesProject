using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Clientes.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SistemaController : ControllerBase
    {
        private class AplicacaoInfo
        {
            public string Maquina { get; set; }
            public string Host { get; set; }
            public string OS { get; set; }

            public AplicacaoInfo(string maquina, string host, string os)
            {
                Maquina = maquina;
                Host = host;
                OS = os;
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("informacoes-ambiente")]
        public IActionResult Ambiente()
        {
            var appInfo = new AplicacaoInfo(Environment.MachineName, HttpContext.Request.Host.ToString(), Environment.OSVersion.VersionString);

            return Ok(appInfo);
        }
    }
}