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
using MapperLib;
using MapperLib.Abstract;

namespace ConsoleTest
{
    static class Program
    {
        private static async Task Main(string[] args)
        {
            //Recipient[] result;

            //using (IDbContext context = new MailSenderDbContext())
            //{

            //    result = await context.Set<Recipient>().ToArrayAsync();
            //}

            //global::Repository.RecipientRepository recipientRepository = new global::Repository.RecipientRepository();

            //var result = await recipientRepository.FindByAsync(r => r.Id >= 5 && r.Id <= 8);

            //Repository<Recipient> repository = new Repository<Recipient>();

            //var result = await repository.FindByAsync(r => r.Id >= 5 && r.Id <= 8);

            //IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(typeof(MailSenderDbContext));

            //IUnitOfWork<Recipient> unitOfWork = unitOfWorkFactory.GetUnitOfWork<Recipient>();

            //unitOfWork.Print();

            //int result1 = await unitOfWork.AddAsync(new Recipient { Id = 0, Name = "id 17" });
            //Console.WriteLine();
            //Console.WriteLine($" AddAsync {result1}");
            //Console.WriteLine();

            //unitOfWork.Print();

            //bool result11 = await unitOfWork.UpdateAsync(new Recipient { Id = 15, Name = "Update" });
            //Console.WriteLine();
            //Console.WriteLine($" UpdateAsync {result11}");
            //Console.WriteLine();

            //unitOfWork.Print();

            //bool result2 = await unitOfWork.DeleteAsync(new Recipient { Id = 16 });
            //Console.WriteLine();
            //Console.WriteLine($" DeleteAsync {result2}");
            //Console.WriteLine();

            //unitOfWork.Print();

            RecipientDomain recipientDomain = new RecipientDomain{NameFull = "Domain", Description = "DescriptionDomain", ListsId = new List<int>{3, 4}};

            //var config = new MapperConfiguration(cfg => cfg.CreateMap<RecipientDomain, Recipient>()
            //    .ForMember(dist => dist.Name, act => act.MapFrom(source => source.NameFull))
            //    .ForMember(dist => dist.Description, act => act.Ignore())
            //    .ReverseMap()
            //    .ForMember(dist => dist.Description, act => act.Ignore())
            //);

            IMapperFactory mapperFactory = new MapperFactory(MappingConfig.GetMappingTypes(), MappingConfig.GetConfigExpressions());

            IMapper mapRecipient = mapperFactory.GetMapper<RecipientDomain>();
            var entity = mapRecipient.Map<Recipient>(recipientDomain);


            Console.WriteLine($"Entity: id = {entity.Id}, Name = {entity.Name}, Description = {entity.Description}");
            foreach (var list in entity.ListsId)
            {
                Console.WriteLine($"ListsId = {list}");
            }


            Recipient recipient = new Recipient { Name = "Entity", Description = "DescriptionEntity", ListsId = new List<int> { 7, 8 } };

            var domain = mapRecipient.Map<RecipientDomain>(recipient);

            Console.WriteLine($"Domain: id = {domain.Id}, Name = {domain.NameFull}, Description = {domain.Description}");
            foreach (var list in domain.ListsId)
            {
                Console.WriteLine($"ListsId = {list}");
            }

            recipient.ListsId.Add(10);
            recipient.ListsId.Remove(8);

            foreach (var list in domain.ListsId)
            {
                Console.WriteLine($"После изменений в Entity: ListsId = {list}");
            }

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
    }
}
