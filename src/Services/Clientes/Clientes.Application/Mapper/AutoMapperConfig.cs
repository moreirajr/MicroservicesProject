using AutoMapper;

namespace Clientes.Application.Mapper
{
    public class AutoMapperConfig
    {
        private IMapper _mapper;

        public AutoMapperConfig()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
        }

        public IMapper Mapper => _mapper;
    }
}