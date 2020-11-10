using System.Collections.Generic;

namespace Contas.Domain.Models
{
    public class Conta
    {
        public long Id { get; set; }

        public long ClienteId { get; set; }
        public ClienteConta Cliente { get; set; }

        public ICollection<Cartao> Cartoes { get; set; }

        public decimal Saldo { get; set; }

        public Conta()
        {
            Cartoes = new List<Cartao>();
        }

        public Conta(ClienteConta cliente) : this()
        {
            Cliente = cliente;
            Saldo = 0;
        }
    }
}