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
        public DbSet<RecipientEntity> Recipients { get; set; }

        public DbSet<SenderEntity> Senders { get; set; }

        public DbSet<ServerEntity> Servers { get; set; }

        public DbSet<MailMessageEntity> MailMessages { get; set; }

        public DbSet<RecipientsListEntity> Lists { get; set; }

        public DbSet<SchedulerTaskEntity> SchedulerTasks { get; set; }


        static MailSenderDbContext() => System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<MailSenderDbContext, Configuration>());
        public MailSenderDbContext(string connectionString) : base(connectionString) { }

        public MailSenderDbContext() : base("Name=MailSenderDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.Log = (s => Console.WriteLine(s));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RecipientEntityConfiguration());
            modelBuilder.Configurations.Add(new RecipientsListEntityConfiguration());
            modelBuilder.Configurations.Add(new MailMessageEntityConfiguration());
            modelBuilder.Configurations.Add(new SchedulerTaskEntityConfiguration());
            modelBuilder.Configurations.Add(new SenderEntityConfiguration());
            modelBuilder.Configurations.Add(new ServerEntityConfiguration());
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }


        IQueryable<TEntity> IDbContext.Set<TEntity>() => Set<TEntity>();


        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            Set<TEntity>().Attach(entity);
            Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            Set<TEntity>().Attach(entity);
            Entry(entity).State = EntityState.Modified;
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            Set<TEntity>().Attach(entity);
            Set<TEntity>().Remove(entity);
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().AddRange(entities);
        }

        public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;

            foreach (TEntity entity in entities)
            {
                Entry(entity).State = EntityState.Modified;
                Set<TEntity>().Attach(entity);
            }

            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ValidateOnSaveEnabled = true;
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;

            foreach (TEntity entity in entities)
            {
                Entry(entity).State = EntityState.Deleted;
                Set<TEntity>().Attach(entity);
            }

            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ValidateOnSaveEnabled = true;
        }

        async Task<int> IDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            int numberOfSavedChanges = await SaveChangesAsync(cancellationToken);

            return numberOfSavedChanges;
        }
    }
}
