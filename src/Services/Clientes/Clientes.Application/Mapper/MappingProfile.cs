using AutoMapper;
using Clientes.Application.Clientes.ViewModels;
using Clientes.Domain.Clientes;
using Clientes.Domain.MongoModels;
using Clientes.Domain.Pessoas;
using System.Linq;

namespace Clientes.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Telefone, TelefoneVM>().ReverseMap();
            CreateMap<Endereco, EnderecoVM>().ReverseMap();

            CreateMap<Pessoa, PessoaCadastroVM>()
                .ForMember(dest => dest.EnderecoPadrao, opt => opt.MapFrom(src => src.Enderecos.FirstOrDefault()))
                .ForMember(dest => dest.TelefonePadrao, opt => opt.MapFrom(src => src.Telefones.FirstOrDefault()))
                .ReverseMap();

            CreateMap<Cliente, ClienteCadastroVM>().ReverseMap();

            #region Mongo Models
            CreateMap<ClienteDocument, ClienteVM>().ReverseMap();
            #endregion
        }
    }
}