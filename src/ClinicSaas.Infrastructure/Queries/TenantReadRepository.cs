using ClinicSaas.Application.Abstractions;
using ClinicSaas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSaas.Infrastructure.Queries;

public sealed class TenantReadRepository(ApplicationDbContext dbContext) : ITenantReadRepository
{
    public async Task<TenantInfoDto?> GetTenantInfoAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tenants
            .Where(x => x.Id == tenantId)
            .Select(x => new TenantInfoDto(
                x.Id,
                x.Name,
                x.Subdomain,
                x.ContactEmail,
                x.CreatedAtUtc,
                null,
                null,
                null))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
