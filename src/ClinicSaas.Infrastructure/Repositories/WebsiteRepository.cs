using ClinicSaas.Application.Abstractions;
using ClinicSaas.Domain.Entities.Website;
using ClinicSaas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicSaas.Infrastructure.Repositories;

public sealed class WebsiteRepository(ApplicationDbContext dbContext) : IWebsiteRepository
{
    public async Task<Website?> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Websites
            .Include(w => w.Pages)
            .ThenInclude(p => p.Sections)
            .AsNoTracking()
            .SingleOrDefaultAsync(w => w.TenantId == tenantId, cancellationToken);
    }

    public async Task AddAsync(Website website, CancellationToken cancellationToken = default)
    {
        await dbContext.Websites.AddAsync(website, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Website> EnsureDefaultWebsiteAsync(Guid tenantId, string theme, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Websites
            .Include(x => x.Pages)
            .ThenInclude(x => x.Sections)
            .FirstOrDefaultAsync(x => x.TenantId == tenantId, cancellationToken);
        if (existing is not null)
        {
            return existing;
        }

        var website = new Website
        {
            TenantId = tenantId,
            Theme = string.IsNullOrWhiteSpace(theme) ? "default-light" : theme.Trim()
        };
        var page = new Page
        {
            TenantId = tenantId,
            WebsiteId = website.Id,
            Slug = "home",
            Title = "Home"
        };
        page.Sections.Add(new Section
        {
            TenantId = tenantId,
            PageId = page.Id,
            Type = "hero",
            ContentJson = """{"title":"Welcome","subtitle":"Clinic care made simple","ctaText":"Book now"}""",
            SortOrder = 1
        });
        website.Pages.Add(page);

        await dbContext.Websites.AddAsync(website, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return website;
    }
}
