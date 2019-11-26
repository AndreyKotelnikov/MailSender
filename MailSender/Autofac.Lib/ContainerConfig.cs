using CodeFirstDbContext;
using CodeFirstDbContext.Abstract;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Abstract;
using MapperLib;
using AutoMapper;

namespace Autofac.Lib
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CodeFirstDbContextProvider>()
                .As<IDbContextProvider>()
                .WithParameter("typeDbContext", typeof(MailSenderDbContext))
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWorkFactory>()
                .As<IUnitOfWorkFactory>()
                .InstancePerLifetimeScope();
            builder.Register(c => MappingProfile.InitializeAutoMapper().CreateMapper())
                .As<IMapper>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
