using CodeFirstDbContext;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;
using Repository;
using Repository.Abstract;

namespace ConsoleTest
{
    class Program
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

            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(typeof(MailSenderDbContext));

            IUnitOfWork<Recipient> unitOfWork = unitOfWorkFactory.GetUnitOfWork<Recipient>();

            var result = await unitOfWork.FindByAsync(r => r.Id >= 5 && r.Id <= 8);

            foreach (var recipient in result)
            {
                Console.WriteLine($"{recipient.Id} => {recipient.Name}");
            }

            Console.ReadLine();
        }
    }
}
