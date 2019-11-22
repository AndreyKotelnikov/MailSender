using System.Data.Entity.ModelConfiguration;
using Entities;

namespace CodeFirstDbContext.EntityConfigurations
{
    public class RecipientsListEntityConfiguration : EntityTypeConfiguration<RecipientsListEntity>
    {
        public RecipientsListEntityConfiguration()
        {
            HasEntitySetName("RecipientsList");

            HasKey(r => r.Id);

            Property(r => r.CreatedDate)
                .IsRequired();

            Property(p => p.RowVersion)
                .IsRowVersion();

            Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(255);

            HasMany(r => r.SchedulerTaskEntity)
                .WithRequired(m => m.RecipientsListEntity)
                .HasForeignKey(m => m.ListId);
                
            HasMany(r => r.RecipientEntities)
                .WithMany(m => m.RecipientsListEntities);
        }
    }
}