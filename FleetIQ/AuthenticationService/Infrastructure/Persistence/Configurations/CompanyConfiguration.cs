using AuthenticationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Set primary key
            builder.HasKey(c => c.Id);

            // Define properties
            builder.Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.OwnerId)
                .IsRequired();

            builder.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(255);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.State).HasMaxLength(100);
                address.Property(a => a.Country).HasMaxLength(100);
                address.Property(a => a.PostalCode).HasMaxLength(20);
            });

            // Define relationships
            builder.HasMany(c => c.Users)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}
