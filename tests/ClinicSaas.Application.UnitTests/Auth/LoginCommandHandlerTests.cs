using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Auth.Commands;
using ClinicSaas.Application.Auth.Dtos;
using ClinicSaas.Domain.Entities.Identity;
using FluentAssertions;
using NSubstitute;

namespace ClinicSaas.Application.UnitTests.Auth;

public sealed class LoginCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Fails_WhenTenantContextMissing()
    {
        var userRepository = Substitute.For<IUserRepository>();
        var tenantContext = Substitute.For<ITenantContext>();
        tenantContext.TenantId.Returns((Guid?)null);
        var jwtGenerator = Substitute.For<IJwtTokenGenerator>();

        var sut = new LoginCommandHandler(userRepository, tenantContext, jwtGenerator);
        var result = await sut.HandleAsync(new LoginCommand("admin@clinic.com", "Pass123!"));

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.Message.Contains("Tenant header is required."));
    }

    [Fact]
    public async Task HandleAsync_ReturnsTokens_WhenCredentialsAreValid()
    {
        var tenantId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var userRepository = Substitute.For<IUserRepository>();
        var tenantContext = Substitute.For<ITenantContext>();
        tenantContext.TenantId.Returns(tenantId);
        var jwtGenerator = Substitute.For<IJwtTokenGenerator>();

        var user = new ApplicationUser
        {
            Id = userId,
            Email = "owner@clinic.com",
            TenantId = tenantId,
            FullName = "Clinic Owner",
            TenantSubdomain = "my-clinic"
        };

        userRepository.GetByEmailAsync(tenantId, "owner@clinic.com", Arg.Any<CancellationToken>())
            .Returns(user);
        userRepository.CheckPasswordAsync(user, "StrongPass123!").Returns(true);
        userRepository.GetRolesAsync(user, Arg.Any<CancellationToken>())
            .Returns(new[] { ApplicationRole.TenantAdmin });
        jwtGenerator.Generate(user, Arg.Any<IReadOnlyCollection<string>>())
            .Returns(new AuthTokensDto("jwt-token", DateTimeOffset.UtcNow.AddHours(1)));

        var sut = new LoginCommandHandler(userRepository, tenantContext, jwtGenerator);
        var result = await sut.HandleAsync(new LoginCommand("owner@clinic.com", "StrongPass123!"));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.UserId.Should().Be(userId);
        result.Value.Tokens.AccessToken.Should().Be("jwt-token");
    }
}
