using MediatR;

namespace Clientes.Domain.DomainEvents
{
    public class ClienteCriadoDomainEvent : INotification
    {
        public long Id { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }

        public ClienteCriadoDomainEvent(long id, string cpf, string nome)
        {
            Id = id;
            CPF = cpf;
            Nome = nome;
        }
    }
}