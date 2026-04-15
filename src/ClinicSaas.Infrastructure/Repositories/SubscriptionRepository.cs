using ClinicSaas.Application.Abstractions;
using ClinicSaas.Domain.Entities.Subscription;
using ClinicSaas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSaas.Infrastructure.Repositories;

public sealed class SubscriptionRepository(ApplicationDbContext dbContext) : ISubscriptionRepository
{
    public async Task<Plan?> GetPlanByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToLowerInvariant();
        return await dbContext.Plans.FirstOrDefaultAsync(x => x.Code == normalizedCode, cancellationToken);
    }

    public async Task AddSubscriptionAsync(TenantSubscription subscription, CancellationToken cancellationToken = default)
    {
        await dbContext.Subscriptions.AddAsync(subscription, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> HasAnyPlansAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Plans.AnyAsync(cancellationToken);
    }

    public async Task SeedPlansAsync(IEnumerable<Plan> plans, CancellationToken cancellationToken = default)
    {
        await dbContext.Plans.AddRangeAsync(plans, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TenantHasFeatureAsync(Guid tenantId, string featureCode, CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.Subscriptions
            .AsNoTracking()
            .Include(x => x.Plan)
            .OrderByDescending(x => x.StartsAtUtc)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Status == "Active", cancellationToken);

        if (subscription?.Plan is null)
        {
            return false;
        }

        return featureCode.ToLowerInvariant() switch
        {
            "websitebuilder" => subscription.Plan.IsWebsiteBuilderEnabled,
            "aiassistant" => subscription.Plan.IsAiAssistantEnabled,
            "smartscheduling" => subscription.Plan.IsSmartSchedulingEnabled,
            _ => false
        };
    }
}
