using System;

namespace Contas.Domain.Models
{
    public class Cartao
    {
        public long Id { get; set; }

        public long ContaId { get; set; }

        public string Numero { get; set; }

        public string Codigo { get; set; }

        public bool Bloqueado { get; set; }

        public Cartao() { }

        public Cartao(long contaId)
        {
            ContaId = contaId;

            var rnd = new Random();

            Numero = $"{rnd.Next(1000, 9999)}{rnd.Next(1000, 9999)}{rnd.Next(1000, 9999)}{rnd.Next(1000, 9999)}";
            Codigo = $"{rnd.Next(100, 999)}";
            Bloqueado = true;
        }

        public override string ToString()
        {
            return $"****-****-****-{Numero.Substring(12)}";
        }
    }
}