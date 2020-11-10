using Contas.Application.CQRS.Commands;
using Contas.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contas.Application.CQRS.CommandsHanlders
{
    public class AtivarCartaoCommandHandler : IRequestHandler<AtivarCartaoCommand, bool>
    {
        private readonly IContaCommandRepository _contaRepository;

        public AtivarCartaoCommandHandler(IContaCommandRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }
        public Task<bool> Handle(AtivarCartaoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}