using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace MapperLib
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ClientMappingProfile());  //mapping between Client and Business layer objects
                cfg.AddProfile(new BusinessLogicMappingProfile());  // mapping between Business and Data layer objects
            });

            return config;
        }
    }
}
