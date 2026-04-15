using ClinicSaas.Application.Abstractions;
using ClinicSaas.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ClinicSaas.Infrastructure.Repositories;

public sealed class RoleRepository(RoleManager<ApplicationRole> roleManager) : IRoleRepository
{
    public async Task<bool> ExistsAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await roleManager.RoleExistsAsync(roleName);
    }

    public async Task<ApplicationRole?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await roleManager.FindByNameAsync(roleName);
    }

    public async Task EnsureRoleExistsAsync(string roleName, CancellationToken cancellationToken = default)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new ApplicationRole(roleName));
        }
    }

    public async Task EnsureRolesExistAsync(IEnumerable<string> roles, CancellationToken cancellationToken = default)
    {
        foreach (var role in roles.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            await EnsureRoleExistsAsync(role, cancellationToken);
        }
    }
}
