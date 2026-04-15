using ClinicSaas.Domain.Entities.Subscription;

namespace ClinicSaas.Application.Abstractions;

public interface ISubscriptionRepository
{
    Task<Plan?> GetPlanByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> HasAnyPlansAsync(CancellationToken cancellationToken = default);
    Task SeedPlansAsync(IEnumerable<Plan> plans, CancellationToken cancellationToken = default);
    Task AddSubscriptionAsync(TenantSubscription subscription, CancellationToken cancellationToken = default);
    Task<bool> TenantHasFeatureAsync(Guid tenantId, string featureCode, CancellationToken cancellationToken = default);
}
