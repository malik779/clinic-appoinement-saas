using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Clinic;

public sealed class Patient : TenantEntity
{
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}
