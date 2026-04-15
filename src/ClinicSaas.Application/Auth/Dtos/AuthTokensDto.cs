namespace ClinicSaas.Application.Auth.Dtos;

public sealed record AuthTokensDto(string AccessToken, DateTimeOffset ExpiresAtUtc);
