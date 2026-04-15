using ClinicSaas.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace ClinicSaas.Domain.Entities.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>, ITenantOwned
{
    public Guid TenantId { get; set; }
    public string TenantSubdomain { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public static ApplicationUser Create(string email, string fullName, Guid tenantId, string tenantSubdomain)
    {
        return new ApplicationUser
        {
            Email = email.Trim().ToLowerInvariant(),
            UserName = email.Trim().ToLowerInvariant(),
            FullName = fullName.Trim(),
            TenantId = tenantId,
            TenantSubdomain = tenantSubdomain.Trim().ToLowerInvariant(),
            SecurityStamp = Guid.NewGuid().ToString("N")
        };
    }
}
