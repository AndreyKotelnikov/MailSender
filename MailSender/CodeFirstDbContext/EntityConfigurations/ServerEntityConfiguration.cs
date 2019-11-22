using System.Data.Entity.ModelConfiguration;
using Entities;

namespace CodeFirstDbContext.EntityConfigurations
{
    public class ServerEntityConfiguration : EntityTypeConfiguration<ServerEntity>
    {
        public ServerEntityConfiguration()
        {
            HasEntitySetName("Server");

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

            Property(r => r.Port)
                .IsRequired();

            Property(r => r.UseSSL)
                .IsRequired();

            Property(r => r.UserName)
                .HasMaxLength(255);

            HasMany(r => r.SchedulerTaskEntity)
                .WithRequired(m => m.ServerEntity)
                .HasForeignKey(r => r.ServerId);
        }
    }
}