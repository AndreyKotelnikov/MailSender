using System.Data.Entity.ModelConfiguration;
using Entities;

namespace CodeFirstDbContext.EntityConfigurations
{
    public class MailMessageEntityConfiguration : EntityTypeConfiguration<MailMessageEntity>
    {
        public MailMessageEntityConfiguration()
        {
            HasEntitySetName("MailMessage");

            HasKey(r => r.Id);

            Property(r => r.CreatedDate)
                .IsRequired();

            Property(p => p.RowVersion)
                .IsRowVersion();

            Property(r => r.Subject)
                .IsRequired()
                .HasMaxLength(500);

            Property(r => r.Body)
                .IsOptional()
                .HasMaxLength(3000);

            HasMany(r => r.SchedulerTaskEntity)
                .WithRequired(m => m.MailMessageEntity)
                .HasForeignKey(r => r.MailMessageId);
        }
    }
}