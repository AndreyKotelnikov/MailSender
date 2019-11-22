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
    /// <summary>
    /// Карты для создания меппинга между слоями: BusinessLogic и Data
    /// </summary>
    public class BusinessLogicMappingProfile : Profile
    {
        public BusinessLogicMappingProfile()
        {
            CreateMap<RecipientDomain, RecipientEntity>()
                .ForMember(dist => dist.RecipientsListEntities,
                    act => act.MapFrom(source => source.RecipientsListDomain))
                .ReverseMap();
            CreateMap<RecipientsListDomain, RecipientsListEntity>()
                .ReverseMap();
            CreateMap<SenderDomain, SenderEntity>()
                .ReverseMap();
        }
    }
}
