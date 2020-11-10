using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Application.AutoMapper
{
    public class MapperConfig
    {
        private IMapper _mapper;

        public MapperConfig()
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
