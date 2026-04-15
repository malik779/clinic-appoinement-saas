using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Common;
using ClinicSaas.Application.Tenants.Commands;
using ClinicSaas.Domain.Entities.Clinic;
using ClinicSaas.Domain.Entities.Identity;
using ClinicSaas.Domain.Entities.Subscription;
using ClinicSaas.Domain.Entities.Website;
using FluentAssertions;
using NSubstitute;

namespace ClinicSaas.Application.UnitTests.Tenants;

public sealed class RegisterTenantCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsFailure_WhenSubdomainIsAlreadyInUse()
    {
        var tenantRepository = Substitute.For<ITenantRepository>();
        var userRepository = Substitute.For<IUserRepository>();
        var roleRepository = Substitute.For<IRoleRepository>();
        var subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        var websiteRepository = Substitute.For<IWebsiteRepository>();

        tenantRepository.GetBySubdomainAsync("north-clinic", Arg.Any<CancellationToken>())
            .Returns(new Tenant { Subdomain = "north-clinic", Name = "Existing", ContactEmail = "existing@clinic.com" });

        var sut = new RegisterTenantCommandHandler(
            tenantRepository,
            userRepository,
            roleRepository,
            subscriptionRepository,
            websiteRepository);

        var command = new RegisterTenantCommand(
            "North Clinic",
            "north-clinic",
            "admin@northclinic.com",
            "Owner User",
            "Password@123",
            "starter");

        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.Message.Contains("Subdomain"));
        await subscriptionRepository.DidNotReceiveWithAnyArgs().AddSubscriptionAsync(default!, default);
    }

    [Fact]
    public async Task HandleAsync_ReturnsFailure_WhenPlanDoesNotExist()
    {
        var tenantRepository = Substitute.For<ITenantRepository>();
        var userRepository = Substitute.For<IUserRepository>();
        var roleRepository = Substitute.For<IRoleRepository>();
        var subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        var websiteRepository = Substitute.For<IWebsiteRepository>();

        tenantRepository.GetBySubdomainAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((Tenant?)null);
        userRepository.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success()));
        userRepository.AssignRoleAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success()));
        subscriptionRepository.GetPlanByCodeAsync("starter", Arg.Any<CancellationToken>())
            .Returns((Plan?)null);

        var sut = new RegisterTenantCommandHandler(
            tenantRepository,
            userRepository,
            roleRepository,
            subscriptionRepository,
            websiteRepository);

        var result = await sut.HandleAsync(new RegisterTenantCommand(
            "North Clinic",
            "north-clinic",
            "admin@northclinic.com",
            "Owner User",
            "Password@123",
            "starter"));

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(x => x.Message.Contains("role", StringComparison.OrdinalIgnoreCase));
    }
}
