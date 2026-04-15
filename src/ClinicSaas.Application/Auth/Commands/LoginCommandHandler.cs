using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Auth.Dtos;
using ClinicSaas.Application.Common;

namespace ClinicSaas.Application.Auth.Commands;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    ITenantContext tenantContext,
    IJwtTokenGenerator jwtTokenGenerator)
{
    public async Task<Result<AuthResultDto>> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        if (tenantContext.TenantId is null)
        {
            return Result<AuthResultDto>.Failure("Tenant header is required.");
        }

        var user = await userRepository.GetByEmailAsync(tenantContext.TenantId.Value, command.Email, cancellationToken);
        if (user is null)
        {
            return Result<AuthResultDto>.Failure("Invalid credentials.");
        }

        var signInResult = await userRepository.CheckPasswordAsync(user, command.Password);
        if (!signInResult)
        {
            return Result<AuthResultDto>.Failure("Invalid credentials.");
        }

        var roles = await userRepository.GetRolesAsync(user, cancellationToken);
        var tokens = jwtTokenGenerator.Generate(user, roles);

        var result = new AuthResultDto(
            user.Id,
            user.Email ?? string.Empty,
            user.TenantId,
            roles.ToArray(),
            tokens);

        return Result<AuthResultDto>.Success(result);
    }
}
