namespace ClinicSaas.Infrastructure.Auth;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "ClinicSaas";
    public string Audience { get; set; } = "ClinicSaas.Clients";
    public string Secret { get; set; } = "change-this-secret-for-production-very-long";
    public int ExpiryMinutes { get; set; } = 60;
}
