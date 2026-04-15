namespace ClinicSaas.Application.Abstractions;

public interface ITenantContext
{
    Guid? TenantId { get; set; }
    string? TenantSubdomain { get; set; }
    bool HasTenant { get; }
}
