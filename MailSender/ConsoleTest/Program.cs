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
using Repository.AsyncQueryProvider;

namespace ConsoleTest
{
    static class Program
    {
        private static async Task Main(string[] args)
        {

            #region Тестирование UnitOfWorkEntity

            IDbContextProvider dbContextProvider = new CodeFirstDbContextProvider(typeof(MailSenderDbContext));
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dbContextProvider);

            IUnitOfWork<RecipientEntity> unitOfWork = unitOfWorkFactory.GetCurrentUnitOfWork<RecipientEntity>();

            int result = await unitOfWork.GetAsync(p => p.Count());
            Console.WriteLine();
            Console.WriteLine($" Count {result}");
            Console.WriteLine();


            unitOfWork.Print();

            var rec = new RecipientEntity
            {
                Id = 0,
                Name = "id 17 Entity",
                RecipientsListEntities = new List<RecipientsListEntity>()
                {
                    new RecipientsListEntity{Id = 1, Name = "List 1"},
                    new RecipientsListEntity{Id = 2, Name = "List 2"}
                }
            };

            int result1 = await unitOfWork.AddAsync(rec);
            Console.WriteLine();
            Console.WriteLine($" AddAsync Entity {result1}");
            Console.WriteLine();

            unitOfWork.Print();

            bool result11 = await unitOfWork.UpdateAsync(new RecipientEntity { Id = 20, Name = "UpdateEntity" });
            Console.WriteLine();
            Console.WriteLine($" UpdateAsync Entity {result11}");
            Console.WriteLine();

            unitOfWork.Print();

            bool result2 = await unitOfWork.DeleteAsync(new RecipientEntity { Id = result1 });
            Console.WriteLine();
            Console.WriteLine($" DeleteAsync Entity {result2}");
            Console.WriteLine();

            unitOfWork.Print();

            #endregion

            #region Тестирование UnitOfWorkDomain


            IDbContextProvider dbContextProviderD = new CodeFirstDbContextProvider(typeof(MailSenderDbContext));
            IMapper mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            
            IUnitOfWorkFactory unitOfWorkFactoryD = new UnitOfWorkFactory(dbContextProviderD, mapper);

            IUnitOfWork<RecipientDomain> unitOfWorkD = unitOfWorkFactoryD.GetCurrentUnitOfWork<RecipientDomain>();

            int resultD = await unitOfWorkD.GetAsync(p => p.Count());
            Console.WriteLine();
            Console.WriteLine($" Count {resultD}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            var recipientDomain = new RecipientDomain
            {
                Id = 0,
                Name = "id 20 Domain",
                RecipientsListDomain = new List<RecipientsListDomain>
                {
                    new RecipientsListDomain {Id = 1, Name = "List 1"},
                    new RecipientsListDomain {Id = 2, Name = "List 2"}
                }
            };

            int resultD1 = await unitOfWorkD.AddAsync(recipientDomain);
            Console.WriteLine();
            Console.WriteLine($" AddAsync Domain {resultD1}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            bool result1D1 = await unitOfWorkD.UpdateAsync(new RecipientDomain { Id = 22, Name = "UpdateDomain" });
            Console.WriteLine();
            Console.WriteLine($" UpdateAsync Domain {result1D1}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            bool resultD2 = await unitOfWorkD.DeleteAsync(new RecipientDomain { Id = resultD1 });
            Console.WriteLine();
            Console.WriteLine($" DeleteAsync Domain {resultD2}");
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

        private static void Print<T>(this IUnitOfWork<T> unit) where T: RecipientEntity
        {
            {
                var result = unit.GetAsync(p => p.Include(l => l.RecipientsListEntities).Where(w => w.Id > 40)).Result;

                foreach (var recipient in result)
                {
                    if (recipient.Id >= 41)
                    {
                        ;
                    }
                    Console.WriteLine($"RecipientEntity {recipient.Id} => {recipient.Name}");

                    if (recipient.RecipientsListEntities != null)
                    {
                        foreach (var list in recipient.RecipientsListEntities)
                        {
                            Console.WriteLine($"ListEntity {list.Id} => {list.Name}");
                        }
                    }
                }
            }
        }

        private static void PrintD<T>(this IUnitOfWork<T> unit) where T : RecipientDomain
        {
            {
                
                var result = unit.GetAsync(p => p.Include(l => l.RecipientsListDomain).Where(w => w.Id > 40)).Result;

                foreach (var recipient in result)
                {
                    Console.WriteLine($"RecipientDomain {recipient.Id} => {recipient.Name}");

                    if (recipient.RecipientsListDomain != null)
                    {
                        foreach (var list in recipient.RecipientsListDomain)
                        {
                            Console.WriteLine($"ListDomain {list.Id} => {list.Name}");
                        }
                    }
                }
            }
        }
    }
}
