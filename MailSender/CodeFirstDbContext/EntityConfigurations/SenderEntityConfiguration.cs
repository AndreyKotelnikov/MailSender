using System.Data.Entity.ModelConfiguration;
using Entities;

namespace CodeFirstDbContext.EntityConfigurations
{
    public class SenderEntityConfiguration : EntityTypeConfiguration<SenderEntity>
    {
        public SenderEntityConfiguration()
        {
            HasEntitySetName("Sender");

            HasKey(r => r.Id);

            Property(r => r.CreatedDate)
                .IsRequired();

            Property(p => p.RowVersion)
                .IsRowVersion();

            Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(255);

            Property(r => r.ConnectAdress)
                .HasMaxLength(255);

            Property(r => r.Description)
                .IsOptional()
                .HasMaxLength(2000);

            HasMany(r => r.SchedulerTaskEntity)
                .WithRequired(m => m.SenderEntity)
                .HasForeignKey(r => r.SenderId);
        }
    }
}