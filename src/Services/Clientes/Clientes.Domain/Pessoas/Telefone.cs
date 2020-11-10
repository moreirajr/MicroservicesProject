using System;

namespace Clientes.Domain.Pessoas
{
    public class Telefone : IEquatable<Telefone>
    {
        public long Id { get; set; }
        public string DDI { get; set; }
        public string DDD { get; set; }
        public string Numero { get; set; }
        public bool TelefonePadrao { get; set; }

        public long PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

        public Telefone() { }
        public Telefone(string ddi, string ddd, string numero, bool telefonePadrao = false)
        {
            if (string.IsNullOrEmpty(ddi))
                throw new ArgumentNullException("DDI obrigatório.");

            if (string.IsNullOrEmpty(ddd))
                throw new ArgumentNullException("DDD obrigatório.");

            if (string.IsNullOrEmpty(numero))
                throw new ArgumentNullException("Número obrigatório.");

            DDI = int.TryParse(ddi, out _) ? ddi : throw new ArgumentException("DDI inválido");
            DDD = int.TryParse(ddd, out _) ? ddd : throw new ArgumentException("DDD inválido");
            Numero = int.TryParse(numero, out _) ? numero : throw new ArgumentException("Número inválido");

            TelefonePadrao = telefonePadrao;
        }

        private string DDIFormatado => string.IsNullOrEmpty(DDI) ? "" : $"+{DDI} ";
        private string DDDFormatado => string.IsNullOrEmpty(DDD) ? "" : $"({DDD}) ";
        private string NumeroFormatado => string.IsNullOrEmpty(Numero) ? "" :
            (Numero.Length == 8 ? string.Format("{0:####-####}", int.Parse(Numero)) : string.Format("{0:#####-####}", int.Parse(Numero)));

        public override string ToString()
        {
            return $"{DDIFormatado}{DDDFormatado}{NumeroFormatado}";
        }

        public bool Equals(Telefone other)
        {
            if (other == null)
                return false;

            if (DDD == other.DDD && 
                DDI == other.DDI && 
                Numero == other.Numero)
                return true;

            return false;
        }
    }
}