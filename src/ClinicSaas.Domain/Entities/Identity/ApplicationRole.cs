using Microsoft.AspNetCore.Identity;

namespace ClinicSaas.Domain.Entities.Identity;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName)
    {
        Name = roleName;
        NormalizedName = roleName.ToUpperInvariant();
    }

    public static readonly IReadOnlyCollection<string> DefaultRoles = new[]
    {
        SuperAdmin,
        TenantAdmin,
        Staff
    };

    public const string SuperAdmin = "SuperAdmin";
    public const string TenantAdmin = "TenantAdmin";
    public const string Staff = "Staff";
}
