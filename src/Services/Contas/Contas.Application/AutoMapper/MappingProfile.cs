using AutoMapper;
using Contas.Application.ViewModels;
using Contas.Domain.Models;

namespace Contas.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cartao, CartaoVM>()
                .ForMember(dest => dest.NumeroCartao, opt => opt.MapFrom(src => src.ToString()))
                .ReverseMap();
        }
    }
}