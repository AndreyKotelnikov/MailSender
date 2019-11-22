using System.Data.Entity.ModelConfiguration;
using Entities;

namespace CodeFirstDbContext.EntityConfigurations
{
    public class SchedulerTaskEntityConfiguration : EntityTypeConfiguration<SchedulerTaskEntity>
    {
        public SchedulerTaskEntityConfiguration()
        {
            HasEntitySetName("SchedulerTask");

            HasKey(r => r.Id);

            Property(r => r.CreatedDate)
                .IsRequired();

            Property(p => p.RowVersion)
                .IsRowVersion();

            Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(255);

            Property(r => r.PlanedTime)
                .IsRequired();

            HasRequired(r => r.MailMessageEntity)
                .WithMany(m => m.SchedulerTaskEntity)
                .HasForeignKey(r => r.MailMessageId)
                .WillCascadeOnDelete(false);

            HasRequired(r => r.RecipientsListEntity)
                .WithMany(m => m.SchedulerTaskEntity)
                .HasForeignKey(r => r.ListId)
                .WillCascadeOnDelete(false);

            HasRequired(r => r.SenderEntity)
                .WithMany(m => m.SchedulerTaskEntity)
                .HasForeignKey(r => r.SenderId)
                .WillCascadeOnDelete(false);

            HasRequired(r => r.ServerEntity)
                .WithMany(m => m.SchedulerTaskEntity)
                .HasForeignKey(r => r.ServerId)
                .WillCascadeOnDelete(false);
        }
    }
}