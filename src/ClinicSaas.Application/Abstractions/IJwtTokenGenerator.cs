using ClinicSaas.Domain.Entities.Identity;
using ClinicSaas.Application.Auth.Dtos;

namespace ClinicSaas.Application.Abstractions;

public interface IJwtTokenGenerator
{
    AuthTokensDto Generate(ApplicationUser user, IReadOnlyCollection<string> roles);
}
