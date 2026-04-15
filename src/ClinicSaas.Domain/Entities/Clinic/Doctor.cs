using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Clinic;

public sealed class Doctor : TenantEntity
{
    public required string FullName { get; set; }
    public required string Specialty { get; set; }
}
