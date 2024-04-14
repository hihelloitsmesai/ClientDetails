using ClientDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientDirectory.Infrastructure.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.Property(t => t.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.MobileNumber)
            .HasMaxLength(20);

        builder.Property(t => t.IdNumber)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.PhysicalAddress)
            .HasMaxLength(500)
            .IsRequired();


    }
}
