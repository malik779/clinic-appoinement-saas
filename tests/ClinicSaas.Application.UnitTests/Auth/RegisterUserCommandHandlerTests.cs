using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Auth.Commands;
using ClinicSaas.Application.Common;
using ClinicSaas.Domain.Entities.Clinic;
using ClinicSaas.Domain.Entities.Identity;
using FluentAssertions;
using NSubstitute;

namespace ClinicSaas.Application.UnitTests.Auth;

public sealed class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Fails_WhenTenantContextMissing()
    {
        var tenantContext = Substitute.For<ITenantContext>();
        tenantContext.TenantId.Returns((Guid?)null);
        var currentUserContext = Substitute.For<ICurrentUserContext>();
        var tenantRepository = Substitute.For<ITenantRepository>();
        var userRepository = Substitute.For<IUserRepository>();
        var roleRepository = Substitute.For<IRoleRepository>();

        var sut = new RegisterUserCommandHandler(
            tenantContext,
            currentUserContext,
            tenantRepository,
            userRepository,
            roleRepository);

        var result = await sut.HandleAsync(new RegisterUserCommand(
            "staff@clinic.com",
            "Password123!",
            "Jane Staff",
            ApplicationRole.Staff));

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task HandleAsync_Fails_WhenCallerNotAdmin()
    {
        var tenantId = Guid.NewGuid();
        var tenantContext = Substitute.For<ITenantContext>();
        tenantContext.TenantId.Returns(tenantId);
        var currentUserContext = Substitute.For<ICurrentUserContext>();
        currentUserContext.UserId.Returns(Guid.NewGuid());
        currentUserContext.Roles.Returns(["Staff"]);
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.GetByIdAsync(tenantId, Arg.Any<CancellationToken>()).Returns(new Tenant
        {
            Id = tenantId,
            Name = "Clinic",
            Subdomain = "clinic",
            ContactEmail = "contact@clinic.com"
        });
        var userRepository = Substitute.For<IUserRepository>();
        var roleRepository = Substitute.For<IRoleRepository>();

        var sut = new RegisterUserCommandHandler(
            tenantContext,
            currentUserContext,
            tenantRepository,
            userRepository,
            roleRepository);

        var result = await sut.HandleAsync(new RegisterUserCommand(
            "staff@clinic.com",
            "Password123!",
            "Jane Staff",
            ApplicationRole.Staff));

        result.IsSuccess.Should().BeFalse();
        result.Errors.Select(x => x.Message).Should().Contain("Only admins can register users.");
    }

    [Fact]
    public async Task HandleAsync_CreatesAndAssignsRole_ForValidRequest()
    {
        var tenantId = Guid.NewGuid();
        var tenantContext = Substitute.For<ITenantContext>();
        tenantContext.TenantId.Returns(tenantId);
        var currentUserContext = Substitute.For<ICurrentUserContext>();
        currentUserContext.UserId.Returns(Guid.NewGuid());
        currentUserContext.Roles.Returns([ApplicationRole.TenantAdmin]);
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.GetByIdAsync(tenantId, Arg.Any<CancellationToken>()).Returns(new Tenant
        {
            Id = tenantId,
            Name = "Clinic",
            Subdomain = "clinic",
            ContactEmail = "contact@clinic.com"
        });
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        userRepository.AssignRoleAsync(Arg.Any<ApplicationUser>(), ApplicationRole.Staff, Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        var roleRepository = Substitute.For<IRoleRepository>();
        roleRepository.GetByNameAsync(ApplicationRole.Staff, Arg.Any<CancellationToken>())
            .Returns(new ApplicationRole(ApplicationRole.Staff));

        var sut = new RegisterUserCommandHandler(
            tenantContext,
            currentUserContext,
            tenantRepository,
            userRepository,
            roleRepository);

        var result = await sut.HandleAsync(new RegisterUserCommand(
            "staff@clinic.com",
            "Password123!",
            "Jane Staff",
            ApplicationRole.Staff));

        result.IsSuccess.Should().BeTrue();
        await userRepository.Received(1).CreateAsync(Arg.Any<ApplicationUser>(), "Password123!", Arg.Any<CancellationToken>());
        await userRepository.Received(1).AssignRoleAsync(Arg.Any<ApplicationUser>(), ApplicationRole.Staff, Arg.Any<CancellationToken>());
    }
}
