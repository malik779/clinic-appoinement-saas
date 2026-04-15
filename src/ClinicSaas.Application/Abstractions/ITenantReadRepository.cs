namespace ClinicSaas.Application.Abstractions;

public sealed record TenantInfoDto(
    Guid Id,
    string Name,
    string Subdomain,
    string ContactEmail,
    DateTimeOffset CreatedAtUtc,
    string? SubscriptionStatus,
    string? PlanName,
    DateTimeOffset? TrialEndsAtUtc);

public interface ITenantReadRepository
{
    Task<TenantInfoDto?> GetTenantInfoAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
