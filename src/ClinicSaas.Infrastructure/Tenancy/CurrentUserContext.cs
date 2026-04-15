using ClinicSaas.Application.Abstractions;

namespace ClinicSaas.Infrastructure.Tenancy;

public sealed class CurrentUserContext : ICurrentUserContext
{
    public Guid? UserId { get; set; }
    public string? Email { get; set; }
    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
}
