namespace ClinicSaas.Application.Tenants.Commands;

public sealed record RegisterTenantCommand(
    string ClinicName,
    string Subdomain,
    string AdminEmail,
    string AdminFullName,
    string AdminPassword,
    string PlanCode);
