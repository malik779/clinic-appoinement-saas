using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Common;

namespace ClinicSaas.Application.Websites.Commands;

public sealed class EnsureDefaultWebsiteCommandHandler(IWebsiteRepository websiteRepository)
{
    public async Task<Result<Guid>> HandleAsync(EnsureDefaultWebsiteCommand command, CancellationToken cancellationToken)
    {
        var existing = await websiteRepository.GetByTenantIdAsync(command.TenantId, cancellationToken);
        if (existing is not null)
        {
            return Result<Guid>.Success(existing.Id);
        }

        var website = await websiteRepository.EnsureDefaultWebsiteAsync(command.TenantId, "default-light", cancellationToken);
        return Result<Guid>.Success(website.Id);
    }
}
