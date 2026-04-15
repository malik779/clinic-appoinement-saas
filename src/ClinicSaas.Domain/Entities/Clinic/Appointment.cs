using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Clinic;

public sealed class Appointment : TenantEntity
{
    public Guid DoctorId { get; set; }

    public Guid PatientId { get; set; }

    public DateTime StartsAtUtc { get; set; }

    public DateTime EndsAtUtc { get; set; }

    public string Status { get; set; } = "Pending";
}
