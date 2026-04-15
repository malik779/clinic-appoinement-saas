using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Website;

public class Page : TenantEntity
{
    public Guid WebsiteId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Website Website { get; set; } = default!;
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}
