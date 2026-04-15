using ClinicSaas.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicSaas.Infrastructure.Persistence.Configurations;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users", "identity");
        builder.Property(x => x.FullName).HasMaxLength(256);
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.TenantSubdomain).HasMaxLength(63).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.Email }).IsUnique();
    }
}
