using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Common;

namespace ClinicSaas.Application.Tenants.Queries;

public sealed class GetTenantInfoQueryHandler
{
    private readonly ITenantReadRepository _tenantReadRepository;

    public GetTenantInfoQueryHandler(ITenantReadRepository tenantReadRepository)
    {
        _tenantReadRepository = tenantReadRepository;
    }

    public async Task<Result<GetTenantInfoResponse>> HandleAsync(GetTenantInfoQuery query, CancellationToken cancellationToken = default)
    {
        var tenant = await _tenantReadRepository.GetTenantInfoAsync(query.TenantId, cancellationToken);
        if (tenant is null)
        {
            return Result<GetTenantInfoResponse>.Failure("Tenant not found.");
        }

        return Result<GetTenantInfoResponse>.Success(new GetTenantInfoResponse(
            tenant.Id,
            tenant.Name,
            tenant.Subdomain,
            tenant.ContactEmail,
            tenant.CreatedAtUtc));
    }
}
