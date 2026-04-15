namespace ClinicSaas.Application.Auth.Dtos;

public sealed record AuthResultDto(
    Guid UserId,
    string Email,
    Guid TenantId,
    string[] Roles,
    AuthTokensDto Tokens);
