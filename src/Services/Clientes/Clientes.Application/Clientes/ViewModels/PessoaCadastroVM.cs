using System;

namespace Clientes.Application.Clientes.ViewModels
{
    public class PessoaCadastroVM
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }

        public EnderecoVM EnderecoPadrao { get; set; }
        public TelefoneVM TelefonePadrao { get; set; }
    }
}