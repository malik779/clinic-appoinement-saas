namespace ClinicSaas.Domain.Common;

public interface ITenantOwned
{
    Guid TenantId { get; set; }
}
