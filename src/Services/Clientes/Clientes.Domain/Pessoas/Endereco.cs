namespace Clientes.Domain.Pessoas
{
    public class Endereco
    {
        public long Id { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public bool EnderecoPadrao { get; set; }

        public long PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }

        public Endereco() { }
        public Endereco(string cep, string logradouro, string numero, string complemento, bool enderecoPadrao = false)
        {
            CEP = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            EnderecoPadrao = enderecoPadrao;
        }
    }
}