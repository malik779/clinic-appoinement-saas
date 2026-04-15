using ClinicSaas.Domain.Entities.Website;

namespace ClinicSaas.Application.Abstractions;

public interface IWebsiteRepository
{
    Task<Website?> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<Website> EnsureDefaultWebsiteAsync(Guid tenantId, string theme, CancellationToken cancellationToken = default);
    Task AddAsync(Website website, CancellationToken cancellationToken);
}
