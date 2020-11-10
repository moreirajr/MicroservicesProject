namespace Contas.Domain.Models
{
    public class ClienteConta
    {
        public long Id { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }

        public ClienteConta(long id, string cpf, string nome)
        {
            Id = id;
            CPF = cpf;
            Nome = nome;
        }
    }
}