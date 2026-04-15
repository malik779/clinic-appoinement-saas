using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Website;

public sealed class Website : TenantEntity
{
    public string Theme { get; set; } = string.Empty;
    public ICollection<Page> Pages { get; set; } = new List<Page>();
}
