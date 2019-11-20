using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Entities;

namespace MapperLib
{
    public static class MappingConfig
    {
        /// <summary>
        /// Словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = тип объекта более низкого слоя архитектуры.
        /// Примеры: если нам нужен Mapping между объектами Doman и Entity - то в Key указываем тип из Domain.
        /// Если нам нужен Mapping между объектами Model и Doman - то в Key указываем тип из Model.
        /// </summary>
        private static readonly  Dictionary<Type, Type> _mappingTypes = new Dictionary<Type, Type>
        {
            { typeof(RecipientDomain), typeof(RecipientEntity)},
            { typeof(RecipientsListDomain), typeof(RecipientsListEntity)},
            { typeof(SenderDomain), typeof(SenderEntity)}
        };


        /// <summary>
        /// Словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = выражение для создания конфигурации Mapper.
        /// Примеры: если нам нужен Mapping между объектами Doman и Entity - то в Key указываем тип из Domain.
        /// Если нам нужен Mapping между объектами Model и Doman - то в Key указываем тип из Model.
        /// </summary>
        private static readonly Dictionary<Type, Action<IMapperConfigurationExpression>> _configExpressions = new Dictionary<Type, Action<IMapperConfigurationExpression>>
        {
            { typeof(RecipientDomain), cfg => cfg.CreateMap<RecipientDomain, RecipientEntity>()
                .ForMember(dist => dist.Name, act => act.MapFrom(source => source.Name))
                .ForMember(dist => dist.Description, act => act.Ignore())
                .ReverseMap()
                .ForMember(dist => dist.Description, act => act.Ignore())},
            { typeof(RecipientsListDomain), cfg => cfg.CreateMap<RecipientsListDomain, RecipientsListEntity>().ReverseMap()}
        };
        
        /// <summary>
        /// Возвращает словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = тип объекта более низкого слоя архитектуры.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, Type> GetMappingTypes() => _mappingTypes;

        /// <summary>
        /// Возвращает словарь, где key = тип объекта более высокого слоя архитектуры.
        /// Value = выражение для создания конфигурации Mapper.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, Action<IMapperConfigurationExpression>> GetConfigExpressions() =>
            _configExpressions;
    }
}
