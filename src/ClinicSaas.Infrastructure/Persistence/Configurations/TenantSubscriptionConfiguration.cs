using ClinicSaas.Domain.Entities.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicSaas.Infrastructure.Persistence.Configurations;

internal sealed class TenantSubscriptionConfiguration : IEntityTypeConfiguration<TenantSubscription>
{
    public void Configure(EntityTypeBuilder<TenantSubscription> builder)
    {
        builder.ToTable("Subscriptions", "subscription");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status).HasMaxLength(64).IsRequired();
        builder.Property(x => x.FeaturesJson).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(x => x.StartsAtUtc).IsRequired();
        builder.Property(x => x.TrialStartsAtUtc);
        builder.Property(x => x.TrialEndsAtUtc);
        builder.HasIndex(x => new { x.TenantId, x.Status });

        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
