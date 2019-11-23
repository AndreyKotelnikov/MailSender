using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;
using CodeFirstDbContext.EntityConfigurations;
using CodeFirstDbContext.Migrations;
using Entities;
using Entities.Abstract;

namespace CodeFirstDbContext
{
    public class MailSenderDbContext : DbContext, IDbContext
    {
        static MailSenderDbContext() => System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<MailSenderDbContext, Configuration>());

        public MailSenderDbContext() : base("Name=MailSenderDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.Log = (s => Console.WriteLine(s));
        }

        public MailSenderDbContext(string connectionString) : base(connectionString) { }

        public DbSet<RecipientEntity> Recipients { get; set; }

        public DbSet<SenderEntity> Senders { get; set; }

        public DbSet<ServerEntity> Servers { get; set; }

        public DbSet<MailMessageEntity> MailMessages { get; set; }

        public DbSet<RecipientsListEntity> Lists { get; set; }

        public DbSet<SchedulerTaskEntity> SchedulerTasks { get; set; }


        IQueryable<TEntity> IDbContext.Set<TEntity>() 
        {
            return this.Set<TEntity>();
        }


        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Attach(entity);
            this.Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Attach(entity);
            this.Entry(entity).State = EntityState.Modified;
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Attach(entity);
            this.Set<TEntity>().Remove(entity);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RecipientEntityConfiguration());
            modelBuilder.Configurations.Add(new RecipientsListEntityConfiguration());
            modelBuilder.Configurations.Add(new MailMessageEntityConfiguration());
            modelBuilder.Configurations.Add(new SchedulerTaskEntityConfiguration());
            modelBuilder.Configurations.Add(new SenderEntityConfiguration());
            modelBuilder.Configurations.Add(new ServerEntityConfiguration());
        }
    }
}
