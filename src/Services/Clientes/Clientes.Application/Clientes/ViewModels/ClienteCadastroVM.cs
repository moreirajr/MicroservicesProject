using Clientes.Domain.Enums;
using System;

namespace Clientes.Application.Clientes.ViewModels
{
    public class ClienteCadastroVM
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public int StatusId { get; set; }
        public string Status => EStatusCliente.FromId(StatusId)?.Nome;

        public int TipoClienteId { get; set; }
        public string TipoCliente => ETipoCliente.FromId(TipoClienteId)?.Nome;

        public long PessoaId { get; set; }
        public PessoaCadastroVM Pessoa { get; set; }
    }
}