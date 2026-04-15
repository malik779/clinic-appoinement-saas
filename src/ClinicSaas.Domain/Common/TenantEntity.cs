namespace ClinicSaas.Domain.Common;

public abstract class TenantEntity : Entity, ITenantOwned
{
    public Guid TenantId { get; set; }
}
