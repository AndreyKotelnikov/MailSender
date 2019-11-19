using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeFirstDbContext.Abstract;
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

        public DbSet<Recipient> Recipients { get; set; }

        public DbSet<Sender> Senders { get; set; }

        public DbSet<Server> Servers { get; set; }

        public DbSet<MailMessage> MailMessages { get; set; }

        public DbSet<RecipientsList> Lists { get; set; }

        public DbSet<SchedulerTask> SchedulerTasks { get; set; }


        IQueryable<TEntity> IDbContext.Set<TEntity>() 
        {
            return this.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Attach(entity);
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            this.Set<TEntity>().Remove(entity);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
