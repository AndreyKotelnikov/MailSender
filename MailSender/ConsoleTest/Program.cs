using CodeFirstDbContext;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;
using Entities.Abstract;
using Repository;
using Domains;
using Domains.Abstract;
using MapperLib;
using Repository.AsyncQueryProvider;
using Autofac.Lib;
using Autofac;
using RepositoryAbstract;

namespace ConsoleTest
{
    static class Program
    {
        private static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {

                #region Тестирование UnitOfWorkEntity

                //IDbContextProvider dbContextProvider = new CodeFirstDbContextProvider(typeof(MailSenderDbContext));
                //IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(dbContextProvider);

                //IUnitOfWork<RecipientEntity> unitOfWork = unitOfWorkFactory.GetCurrentUnitOfWork<RecipientEntity>();

                //int result = await unitOfWork.GetAsync(p => p.Count());
                //Console.WriteLine();
                //Console.WriteLine($" Count {result}");
                //Console.WriteLine();


                //unitOfWork.Print();

                //var rec = new RecipientEntity
                //{
                //    Id = await unitOfWork.GetMaxIdAsync() + 1,
                //    Name = "id new Entity",
                //    RecipientsListEntities = new List<RecipientsListEntity>()
                //    {
                //        new RecipientsListEntity{Id = 1, Name = "List 1"},
                //        new RecipientsListEntity{Id = 2, Name = "List 2"}
                //    }
                //};

                //int result1 = await unitOfWork.AddAsync(rec);
                //Console.WriteLine();
                //Console.WriteLine($" AddAsync Entity {result1}");
                //Console.WriteLine();

                //unitOfWork.Print();

                //bool result11 = await unitOfWork.UpdateAsync(new RecipientEntity { Id = 20, Name = "UpdateEntity" });
                //Console.WriteLine();
                //Console.WriteLine($" UpdateAsync Entity {result11}");
                //Console.WriteLine();

                //unitOfWork.Print();

                //bool result2 = await unitOfWork.DeleteAsync(new RecipientEntity { Id = result1 });
                //Console.WriteLine();
                //Console.WriteLine($" DeleteAsync Entity {result2}");
                //Console.WriteLine();

                //unitOfWork.Print();

                //var entityList = Enumerable.Range(await unitOfWork.GetMaxIdAsync() + 11, 10)
                //    .Select(e => new RecipientEntity
                //    {
                //        Id = e,
                //        Name = $"EntityList {e}",
                //        RecipientsListEntities = new List<RecipientsListEntity>()
                //        {
                //            new RecipientsListEntity{Id = 1, Name = "List 1"},
                //            new RecipientsListEntity{Id = 2, Name = "List 2"}
                //        }
                //    }).ToList();

                //List<int> entityIdList = await unitOfWork.AddRangeAsync(entityList);
                //Console.WriteLine();
                //foreach (var i in entityIdList)
                //{
                //    Console.WriteLine($" AddRangeAsync Entity {i}");
                //}
                //Console.WriteLine();

                //unitOfWork.Print();

                //entityList.Zip(entityIdList, (f, s) => f.Id == s);

                //bool resultE11 = await unitOfWork.UpdateRangeAsync(entityList);
                //Console.WriteLine();
                //Console.WriteLine($" UpdateRangeAsync Entity {resultE11}");
                //Console.WriteLine();

                //unitOfWork.Print();

                //bool resultE2 = await unitOfWork.DeleteRangeAsync(entityList);
                //Console.WriteLine();
                //Console.WriteLine($" DeleteRangeAsync {resultE2}");
                //Console.WriteLine();

                //unitOfWork.Print();


                #endregion

                Task.WhenAll(TestUnitOfWorkDomain(scope));


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
        }

        private static async Task TestUnitOfWorkDomain(ILifetimeScope scope)
        {
            #region Тестирование UnitOfWorkDomain

            //IDbContextProvider dbContextProviderD = new CodeFirstDbContextProvider(typeof(MailSenderDbContext));
            //IMapper mapper = MappingProfile.InitializeAutoMapper().CreateMapper();

            //IUnitOfWorkFactory unitOfWorkFactoryD = new UnitOfWorkFactory(dbContextProviderD, mapper);

            var unitOfWorkFactoryD = scope.Resolve<IUnitOfWorkFactory>();

            var unitOfWorkD = unitOfWorkFactoryD.GetCurrentUnitOfWork<RecipientDomain>();

            var resultD = await unitOfWorkD.GetAsync(p => p.Count());
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
                        new RecipientsListDomain {Id = 1, Name = "List 1 111"},
                        new RecipientsListDomain {Id = 2, Name = "List 2 111"}
                    }
            };

            var resultD1 = await unitOfWorkD.AddAsync(recipientDomain);
            Console.WriteLine();
            Console.WriteLine($" AddAsync Domain {resultD1}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            var result1D1 = await unitOfWorkD.UpdateAsync(new RecipientDomain { Id = 22, Name = "UpdateDomain" });
            Console.WriteLine();
            Console.WriteLine($" UpdateAsync Domain {result1D1}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            var resultD2 = await unitOfWorkD.DeleteAsync(new RecipientDomain { Id = resultD1 });
            Console.WriteLine();
            Console.WriteLine($" DeleteAsync Domain {resultD2}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            var domainList = Enumerable.Range(await unitOfWorkD.GetMaxIdAsync() + 1001, 10)
                .Select(d => new RecipientDomain()
                {
                    Id = d,
                    Name = $"DomainList {d}",
                    //RecipientsListDomain = new List<RecipientsListDomain>()
                    //{
                    //    new RecipientsListDomain(){Id = 66, Name = "List 1 777"},
                    //    new RecipientsListDomain(){Id = 77, Name = "List 2 777"}
                    //}
                }).ToList();

            var domainIdList = await unitOfWorkD.AddRangeAsync(domainList);
            Console.WriteLine();
            foreach (var i in domainIdList) Console.WriteLine($" AddRangeAsync Domain {i}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            for (var i = 0; i < domainList.Count; i++)
            {
                domainList[i].Id = domainIdList[i];
            }

            var resultD11 = await unitOfWorkD.UpdateRangeAsync(domainList);
            Console.WriteLine();
            Console.WriteLine($" UpdateRangeAsync Domain {resultD11}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            var resultDD2 = await unitOfWorkD.DeleteRangeAsync(domainList);
            Console.WriteLine();
            Console.WriteLine($" DeleteRangeAsync Domain {resultDD2}");
            Console.WriteLine();

            unitOfWorkD.PrintD();

            #endregion
        }


        private static void Print<T>(this IUnitOfWork<T> unit) where T : RecipientEntity
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
