using ClinicSaas.Domain.Entities.Identity;
using ClinicSaas.Application.Common;

namespace ClinicSaas.Application.Abstractions;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email, Guid tenantId, CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);
    Task<Result> AssignRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByIdAsync(Guid tenantId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IReadOnlyCollection<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default);
}
