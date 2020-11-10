using MediatR;

namespace Contas.Application.CQRS.Commands
{
    public class AtivarCartaoCommand : IRequest<bool>
    {
        public long ContaId { get; set; }

        public AtivarCartaoCommand(long contaId)
        {
            ContaId = contaId;
        }
    }
}