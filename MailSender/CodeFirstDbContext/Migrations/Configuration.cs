using System.Collections.Generic;
using Entities;

namespace CodeFirstDbContext.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeFirstDbContext.MailSenderDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CodeFirstDbContext.MailSenderDbContext context)
        {
            if (!context.Recipients.Any())
            {
                var recipients = new Recipient[10];
                for (var i = 0; i < recipients.Length; i++)
                {
                    recipients[i] = new Recipient
                    {
                        CreatedDate = DateTime.Now,
                        Name = $"Получатель {i + 1}",
                        ConnectAdress = $"server_{i + 1}@mail.ru",
                        Description = i % 3 == 0 ? $"Комментарий {i + 1}" : null
                    };
                }

                context.Recipients.AddRange(recipients);
                context.SaveChanges();
            }

            if (!context.Senders.Any())
            {
                Sender[] senders = new Sender[10];
                for (var i = 0; i < senders.Length; i++)
                {
                    senders[i] = new Sender
                    {
                        CreatedDate = DateTime.Now,
                        Name = $"Отправитель {i + 1}",
                        ConnectAdress = $"server_{i + 1}@mail.ru",
                        Description = i % 3 == 0 ? $"Комментарий {i + 1}" : null
                    };
                }

                context.Senders.AddRange(senders);
                context.SaveChanges();
            }

            if (!context.Servers.Any())
            {
                var servers = new Server[10];
                for (var i = 0; i < servers.Length; i++)
                {
                    servers[i] = new Server
                    {
                        CreatedDate = DateTime.Now,
                        Name = $"Сервер {i + 1}",
                        ConnectAdress = $"Адрес_{i + 1}",
                        Port = i * 10,
                        UseSSL = true,
                        UserName = $"Пользователь {i + 1}"
                    };
                }

                context.Servers.AddRange(servers);
                context.SaveChanges();
            }

            if (!context.MailMessages.Any())
            {
                var mailMessages = new MailMessage[10];
                for (var i = 0; i < mailMessages.Length; i++)
                {
                    mailMessages[i] = new MailMessage
                    {
                        CreatedDate = DateTime.Now,
                        Subject = $"Тема_{i + 1}",
                        Body = $"Тело письма {i + 1}"
                    };
                }

                context.MailMessages.AddRange(mailMessages);
                context.SaveChanges();
            }

            if (!context.Lists.Any())
            {
                var recipientsLists = new RecipientsList[2];
                for (var i = 0; i < recipientsLists.Length; i++)
                {
                    recipientsLists[i] = new RecipientsList
                    {
                        CreatedDate = DateTime.Now,
                        Name = $"Лист {i + 1}",
                        Recipients = new List<Recipient>()
                    };

                    foreach (var recipient in context.Recipients.Where(r => r.Id <= (2 + (i * 2))).ToArray())
                    {
                        recipientsLists[i].Recipients.Add(recipient);
                    }
                }

                context.Lists.AddRange(recipientsLists);
                context.SaveChanges();
            }
        }
    }
}
