namespace ClinicSaas.Application.Abstractions;

public interface ICurrentUserContext
{
    Guid? UserId { get; set; }
    string? Email { get; set; }
    IReadOnlyCollection<string> Roles { get; set; }
}
