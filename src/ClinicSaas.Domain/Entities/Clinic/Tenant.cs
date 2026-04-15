using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Clinic;

public sealed class Tenant : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? TimeZone { get; set; }
    public bool IsActive { get; set; } = true;
}
