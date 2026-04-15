namespace ClinicSaas.Application.Abstractions;

using ClinicSaas.Domain.Entities.Identity;

public interface IRoleRepository
{
    Task<bool> ExistsAsync(string roleName, CancellationToken cancellationToken = default);
    Task<ApplicationRole?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
    Task EnsureRoleExistsAsync(string roleName, CancellationToken cancellationToken = default);
    Task EnsureRolesExistAsync(IEnumerable<string> roles, CancellationToken cancellationToken = default);
}
