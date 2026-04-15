using ClinicSaas.Application.Abstractions;

namespace ClinicSaas.Infrastructure.Tenancy;

public sealed class TenantContext : ITenantContext
{
    public Guid? TenantId { get; set; }

    public string? TenantSubdomain { get; set; }

    public bool HasTenant => TenantId.HasValue;
}
