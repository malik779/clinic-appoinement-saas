using ClinicSaas.Domain.Entities.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicSaas.Infrastructure.Persistence.Configurations;

public sealed class WebsiteConfiguration : IEntityTypeConfiguration<Website>
{
    public void Configure(EntityTypeBuilder<Website> builder)
    {
        builder.ToTable("Websites", "website");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Theme).HasMaxLength(128).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => x.TenantId).IsUnique();
        builder.HasMany(x => x.Pages)
            .WithOne(x => x.Website)
            .HasForeignKey(x => x.WebsiteId);
    }
}
