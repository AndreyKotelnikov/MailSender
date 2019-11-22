using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace CodeFirstDbContext.EntityConfigurations
{
    
    public class RecipientEntityConfiguration : EntityTypeConfiguration<RecipientEntity>
    {
        public RecipientEntityConfiguration()
        {
            HasEntitySetName("Recipient");
            
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

            HasMany(r => r.RecipientsListEntities)
                .WithMany(l => l.RecipientEntities)
                .Map(m =>
                {
                    m.ToTable("Recipient_RecipientsList");
                    m.MapLeftKey("RecipientId");
                    m.MapRightKey("ListId");
                });
        }
    }
}
