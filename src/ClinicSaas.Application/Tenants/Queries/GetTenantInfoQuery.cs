namespace ClinicSaas.Application.Tenants.Queries;

public sealed record GetTenantInfoQuery(Guid TenantId);

public sealed record GetTenantInfoResponse(
    Guid Id,
    string Name,
    string Subdomain,
    string ContactEmail,
    DateTimeOffset CreatedAtUtc);
