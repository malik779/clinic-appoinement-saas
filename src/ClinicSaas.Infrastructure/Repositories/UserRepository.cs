using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Common;
using ClinicSaas.Domain.Entities.Identity;
using ClinicSaas.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicSaas.Infrastructure.Repositories;

public sealed class UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<bool> ExistsByEmailAsync(string email, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.NormalizedEmail == email.ToUpperInvariant() && x.TenantId == tenantId, cancellationToken);
    }

    public async Task<Result> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        var identityResult = await userManager.CreateAsync(user, password);
        if (identityResult.Succeeded)
        {
            return Result.Success();
        }

        return Result.Failure(identityResult.Errors.Select(x => x.Description).ToArray());
    }

    public async Task<Result> AssignRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken = default)
    {
        var identityResult = await userManager.AddToRoleAsync(user, roleName);
        if (identityResult.Succeeded)
        {
            return Result.Success();
        }

        return Result.Failure(identityResult.Errors.Select(x => x.Description).ToArray());
    }

    public async Task<ApplicationUser?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Where(x => x.NormalizedEmail == email.ToUpperInvariant() && x.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ApplicationUser?> GetByIdAsync(Guid tenantId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Where(x => x.Id == userId && x.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IReadOnlyCollection<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        var roles = await userManager.GetRolesAsync(user);
        return roles.ToArray();
    }
}
