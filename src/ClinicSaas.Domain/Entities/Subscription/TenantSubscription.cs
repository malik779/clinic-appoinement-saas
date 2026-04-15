using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Subscription;

public sealed class TenantSubscription : TenantEntity
{
    public Guid PlanId { get; set; }
    public string Status { get; set; } = "Trialing";
    public DateTimeOffset StartsAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? EndsAtUtc { get; set; }
    public DateTimeOffset? TrialStartsAtUtc { get; set; }
    public DateTimeOffset? TrialEndsAtUtc { get; set; }
    public string FeaturesJson { get; set; } = "{}";

    public Plan? Plan { get; set; }
}
