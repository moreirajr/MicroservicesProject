using AutoMapper;
using Contas.Application.CQRS.Commands;
using Contas.Application.Interfaces;
using Contas.Application.ViewModels;
using Contas.Domain.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Contas.Application.Services
{
    public class ContaAppService : IContaAppService
    {
        private readonly IMapper _mapper;
        private readonly IContaQueryRepository _contaQueryRepository;
        private readonly IMediator _mediator;

        public ContaAppService(IMapper mapper, IContaQueryRepository contaQueryRepository, IMediator mediator)
        {
            _mapper = mapper;
            _contaQueryRepository = contaQueryRepository;
            _mediator = mediator;
        }

        public async Task<bool> AtivarCartao(long contaId)
        {
            var result = await _mediator.Send(new AtivarCartaoCommand(contaId));
            return result;
        }

        public async Task<CartaoVM> SolicitarCartao(long contaId)
        {
            var result = await _mediator.Send(new SolicitarCartaoCommand(contaId));
            return _mapper.Map<CartaoVM>(result);
        }

        public async Task<ContaClienteVM> ObterContaPorCPF(string cpf)
        {
            throw new NotImplementedException();
        }
    }
}