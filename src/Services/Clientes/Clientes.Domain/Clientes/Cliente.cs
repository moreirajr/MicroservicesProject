using Clientes.Domain.Enums;
using Clientes.Domain.Pessoas;
using Clientes.Domain.SeedWork;
using System;

namespace Clientes.Domain.Clientes
{
    public class Cliente : Entity, IAggregateRoot
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public int StatusId { get; set; }
        public EStatusCliente Status { get; set; }

        public int TipoClienteId { get; set; }
        public ETipoCliente TipoCliente { get; set; }

        public long PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

        public Cliente() { }
        public Cliente(int tipoClienteId, int statusId, Pessoa pessoa)
        {
            TipoClienteId = tipoClienteId;
            Pessoa = pessoa;
            StatusId = statusId;
        }
    }
}