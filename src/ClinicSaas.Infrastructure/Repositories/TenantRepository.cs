using ClinicSaas.Application.Abstractions;
using ClinicSaas.Domain.Entities.Clinic;
using ClinicSaas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSaas.Infrastructure.Repositories;

public sealed class TenantRepository(ApplicationDbContext dbContext) : ITenantRepository
{
    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => dbContext.Tenants.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Tenant?> GetBySubdomainAsync(string subdomain, CancellationToken cancellationToken = default)
        => dbContext.Tenants.FirstOrDefaultAsync(x => x.Subdomain == subdomain.ToLowerInvariant(), cancellationToken);

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await dbContext.Tenants.AddAsync(tenant, cancellationToken);
    }

    public Task<bool> ExistsBySubdomainAsync(string subdomain, CancellationToken cancellationToken = default)
        => dbContext.Tenants.AnyAsync(x => x.Subdomain == subdomain.ToLowerInvariant(), cancellationToken);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
