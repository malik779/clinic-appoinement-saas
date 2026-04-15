using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Website;

public sealed class Section : TenantEntity
{
    public Guid PageId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string ContentJson { get; set; } = "{}";
    public int SortOrder { get; set; }

    public Page? Page { get; set; }
}
