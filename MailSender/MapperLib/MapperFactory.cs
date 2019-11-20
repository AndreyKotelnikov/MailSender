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
        /// <summary>
        /// Словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = тип объекта более низкого слоя архитектуры.
        /// Примеры: если нам нужен Mapping между объектами Doman и Entity - то в Key указываем тип из Domain.
        /// Если нам нужен Mapping между объектами Model и Doman - то в Key указываем тип из Model.
        /// </summary>
        private readonly Dictionary<Type, Type> _mappingTypes;

        /// <summary>
        /// Словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = выражение для создания конфигурации Mapper.
        /// Примеры: если нам нужен Mapping между объектами Doman и Entity - то в Key указываем тип из Domain.
        /// Если нам нужен Mapping между объектами Model и Doman - то в Key указываем тип из Model.
        /// </summary>
        private readonly Dictionary<Type, Action<IMapperConfigurationExpression>> _configExpressions;

        /// <summary>
        /// Конструктор для создания фабрики
        /// </summary>
        /// <param name="mappingConfig">Словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = тип объекта более низкого слоя архитектуры, а Item2 = выражение для создания конфигурации Mapper.
        /// Если Item2 == null, то используется выражение по умолчанию: cfg =&gt; cfg.CreateMap(sourceType, destType).ReverseMap()</param>


        /// <summary>
        /// Конструктор для создания фабрики
        /// </summary>
        /// <param name="mappingTypes">Словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = тип объекта более низкого слоя архитектуры.</param>
        /// <param name="configExpressions">Словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = выражение для создания конфигурации Mapper.
        /// Если значение для типа == null, то используется выражение по умолчанию: cfg =&gt; cfg.CreateMap(sourceType, destType).ReverseMap()</param>
        public MapperFactory(Dictionary<Type, Type> mappingTypes, Dictionary<Type, Action<IMapperConfigurationExpression>> configExpressions = null)
        {
            _mappingTypes = mappingTypes;
            _configExpressions = configExpressions;
        }

        public IMapper GetMapper<TSource>() where TSource : class
        {
            var config = CreateConfiguration(typeof(TSource), _mappingTypes[typeof(TSource)], _configExpressions?[typeof(TSource)]);
            IMapper imap = config.CreateMapper();
            return imap;
        }

        private MapperConfiguration CreateConfiguration(Type sourceType, Type destinationType, Action<IMapperConfigurationExpression> configure)
        {
            MapperConfiguration config = new MapperConfiguration(configure
                ?? (cfg => cfg.CreateMap(sourceType, destinationType)
                .ReverseMap())
            );
            return config;
        }

        
    }
}
