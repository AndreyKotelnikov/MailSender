using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Entities;
using Entities.Abstract;
using MapperLib.Abstract;

namespace MapperLib
{
    public sealed class MapperFactory : IMapperFactory
    {
        private readonly IMapper _mapper;
        
        public MapperFactory()
        {
            _mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
        }

        public Type GetDestinationTypeForDataLayer<TSource>() where TSource : class
        {
            TypeMap[] typeMaps = _mapper.ConfigurationProvider.GetAllTypeMaps();
            return typeMaps.FirstOrDefault(t =>
                t.Profile.Name == typeof(BusinessLogicMappingProfile).FullName
                && t.SourceType == typeof(TSource))
                ?.DestinationType;
        }

        public IMapper GetMapper<TSource>() where TSource : class
        {
            return _mapper;
        }
    }
}
