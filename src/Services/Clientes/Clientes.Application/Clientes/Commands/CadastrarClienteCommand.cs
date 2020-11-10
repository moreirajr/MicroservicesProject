using System;

namespace Clientes.Application.Clientes.Commands
{
    public class CadastrarClienteCommand
    {
        public int TipoClienteId { get; set; }

        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }

        public string DDI { get; set; }
        public string DDD { get; set; }
        public string NumeroTelefone { get; set; }

        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string Complemento { get; set; }
    }
}