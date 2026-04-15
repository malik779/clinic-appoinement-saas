using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Common;
using ClinicSaas.Domain.Entities.Identity;

namespace ClinicSaas.Application.Auth.Commands;

public sealed class RegisterUserCommandHandler(
    ITenantContext tenantContext,
    ICurrentUserContext currentUserContext,
    ITenantRepository tenantRepository,
    IUserRepository userRepository,
    IRoleRepository roleRepository)
{
    private static readonly HashSet<string> AllowedRoles =
    [
        ApplicationRole.TenantAdmin,
        ApplicationRole.Staff
    ];

    public async Task<Result<Guid>> HandleAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        if (tenantContext.TenantId is null)
        {
            return Result<Guid>.Failure("Tenant context is required.");
        }

        var tenant = await tenantRepository.GetByIdAsync(tenantContext.TenantId.Value, cancellationToken);
        if (tenant is null)
        {
            return Result<Guid>.Failure("Tenant not found.");
        }

        var hasPrivilegedRole = currentUserContext.Roles.Contains(ApplicationRole.TenantAdmin) ||
                                currentUserContext.Roles.Contains(ApplicationRole.SuperAdmin);
        if (currentUserContext.UserId.HasValue && !hasPrivilegedRole)
        {
            return Result<Guid>.Failure("Only admins can register users.");
        }

        if (!AllowedRoles.Contains(command.Role))
        {
            return Result<Guid>.Failure("Invalid role.");
        }

        var exists = await userRepository.ExistsByEmailAsync(command.Email, tenant.Id, cancellationToken);
        if (exists)
        {
            return Result<Guid>.Failure("User already exists.");
        }

        var role = await roleRepository.GetByNameAsync(command.Role, cancellationToken);
        if (role is null)
        {
            return Result<Guid>.Failure("Role not found.");
        }

        var user = ApplicationUser.Create(
            command.Email,
            command.FullName,
            tenant.Id,
            tenant.Subdomain);

        var createResult = await userRepository.CreateAsync(user, command.Password, cancellationToken);
        if (!createResult.IsSuccess)
        {
            return Result<Guid>.Failure(createResult.Errors.ToArray());
        }

        var roleResult = await userRepository.AssignRoleAsync(user, command.Role, cancellationToken);
        if (!roleResult.IsSuccess)
        {
            return Result<Guid>.Failure(roleResult.Errors.ToArray());
        }

        return Result<Guid>.Success(user.Id);
    }
}
