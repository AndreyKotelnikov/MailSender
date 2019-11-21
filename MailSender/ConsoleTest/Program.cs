using CodeFirstDbContext;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;
using Entities.Abstract;
using Repository;
using Repository.Abstract;
using Domain;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Abstract;
using MapperLib;
using MapperLib.Abstract;

namespace ConsoleTest
{
    static class Program
    {
        private static async Task Main(string[] args)
        {

            #region Тестирование UnitOfWorkEntity

            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(
                typeof(MailSenderDbContext),
                new MapperFactory(MappingConfig.GetMappingTypes(), MappingConfig.GetConfigExpressions())
            );

            IUnitOfWork<RecipientEntity> unitOfWork = unitOfWorkFactory.GetCurrentUnitOfWork<RecipientEntity>();

            int result = await unitOfWork.GetAsync(p => p.Count());
            Console.WriteLine();
            Console.WriteLine($" Count {result}");
            Console.WriteLine();


            unitOfWork.Print();

            int result1 = await unitOfWork.AddAsync(new RecipientEntity { Id = 0, Name = "id 17 Entity" });
            Console.WriteLine();
            Console.WriteLine($" AddAsync {result1}");
            Console.WriteLine();

            unitOfWork.Print();

            bool result11 = await unitOfWork.UpdateAsync(new RecipientEntity { Id = 20, Name = "UpdateEntity" });
            Console.WriteLine();
            Console.WriteLine($" UpdateAsync {result11}");
            Console.WriteLine();

            unitOfWork.Print();

            bool result2 = await unitOfWork.DeleteAsync(new RecipientEntity { Id = 31 });
            Console.WriteLine();
            Console.WriteLine($" DeleteAsync {result2}");
            Console.WriteLine();

            unitOfWork.Print();

            #endregion

            #region Тестирование UnitOfWorkDomain

            //IUnitOfWorkFactory unitOfWorkFactoryD = new UnitOfWorkFactory(
            //    typeof(MailSenderDbContext),
            //    new MapperFactory(MappingConfig.GetMappingTypes(), MappingConfig.GetConfigExpressions())
            //);

            IUnitOfWork<RecipientDomain> unitOfWorkD = unitOfWorkFactory.GetCurrentUnitOfWork<RecipientDomain>();

            int resultD = await unitOfWork.GetAsync(p => p.Count());
            Console.WriteLine();
            Console.WriteLine($" Count {resultD}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            int resultD1 = await unitOfWorkD.AddAsync(new RecipientDomain { Id = 0, Name = "id 20 Domain" });
            Console.WriteLine();
            Console.WriteLine($" AddAsync {resultD1}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            bool result1D1 = await unitOfWorkD.UpdateAsync(new RecipientDomain { Id = 22, Name = "UpdateDomain" });
            Console.WriteLine();
            Console.WriteLine($" UpdateAsync {result1D1}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            bool resultD2 = await unitOfWorkD.DeleteAsync(new RecipientDomain { Id = 32 });
            Console.WriteLine();
            Console.WriteLine($" DeleteAsync {resultD2}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            #endregion

            #region Тестирование Mapper

            //RecipientDomain recipientDomain = new RecipientDomain{NameFull = "Domain", Description = "DescriptionDomain", ListsId = new List<int>{3, 4}};

            ////var config = new MapperConfiguration(cfg => cfg.CreateMap<RecipientDomain, Recipient>()
            ////    .ForMember(dist => dist.Name, act => act.MapFrom(source => source.NameFull))
            ////    .ForMember(dist => dist.Description, act => act.Ignore())
            ////    .ReverseMap()
            ////    .ForMember(dist => dist.Description, act => act.Ignore())
            ////);

            //IMapperFactory mapperFactory = new MapperFactory(MappingConfig.GetMappingTypes(), MappingConfig.GetConfigExpressions());

            //IMapper mapRecipient = mapperFactory.GetMapper<RecipientDomain>();
            //var entity = mapRecipient.Map<RecipientEntity>(recipientDomain);


            //Console.WriteLine($"Entity: id = {entity.Id}, Name = {entity.Name}, Description = {entity.Description}");
            //foreach (var list in entity.ListsId)
            //{
            //    Console.WriteLine($"ListsId = {list}");
            //}


            //RecipientEntity recipient = new RecipientEntity { Name = "Entity", Description = "DescriptionEntity", ListsId = new List<int> { 7, 8 } };

            //var domain = mapRecipient.Map<RecipientDomain>(recipient);

            //Console.WriteLine($"Domain: id = {domain.Id}, Name = {domain.NameFull}, Description = {domain.Description}");
            //foreach (var list in domain.ListsId)
            //{
            //    Console.WriteLine($"ListsId = {list}");
            //}

            //recipient.ListsId.Add(10);
            //recipient.ListsId.Remove(8);

            //foreach (var list in domain.ListsId)
            //{
            //    Console.WriteLine($"После изменений в Entity: ListsId = {list}");
            //}

            #endregion

            Console.ReadLine();
        }

        private static void Print<T>(this IUnitOfWork<T> unit) where T: NamedEntity
        {
            {
                var result = unit.GetAllAsync().Result;

                foreach (var recipient in result)
                {
                    Console.WriteLine($"{recipient.Id} => {recipient.Name}");
                }
            }
        }

        private static void PrintD<T>(this IUnitOfWork<T> unit) where T : NamedDomain
        {
            {
                var result = unit.GetAllAsync().Result;

                foreach (var recipient in result)
                {
                    Console.WriteLine($"{recipient.Id} => {recipient.Name}");
                }
            }
        }
    }
}
