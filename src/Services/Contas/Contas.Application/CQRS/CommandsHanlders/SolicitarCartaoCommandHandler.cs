using Contas.Application.CQRS.Commands;
using Contas.Domain.Interfaces;
using Contas.Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Contas.Application.CQRS.CommandsHanlders
{
    public class SolicitarCartaoCommandHandler : IRequestHandler<SolicitarCartaoCommand, Cartao>
    {
        private readonly IContaCommandRepository _contaCommandRepository;

        public SolicitarCartaoCommandHandler(IContaCommandRepository contaCommandRepository)
        {
            _contaCommandRepository = contaCommandRepository;
        }

        public async Task<Cartao> Handle(SolicitarCartaoCommand request, CancellationToken cancellationToken)
        {
            return await _contaCommandRepository.SolicitarCartao(request.ContaId);
        }
    }
}