using Contas.Domain.Models;
using MediatR;

namespace Contas.Application.CQRS.Commands
{
    public class SolicitarCartaoCommand : IRequest<Cartao>
    {
        public long ContaId { get; set; }

        public SolicitarCartaoCommand(long contaId)
        {
            ContaId = contaId;
        }
    }
}