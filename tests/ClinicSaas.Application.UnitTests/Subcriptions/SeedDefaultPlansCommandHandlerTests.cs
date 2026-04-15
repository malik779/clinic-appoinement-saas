using System.Linq;
using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Subscriptions.Commands;
using ClinicSaas.Domain.Entities.Subscription;
using NSubstitute;

namespace ClinicSaas.Application.UnitTests.Subscriptions;

public sealed class SeedDefaultPlansCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DoesNothing_WhenPlansAlreadyExist()
    {
        var subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        subscriptionRepository.HasAnyPlansAsync(Arg.Any<CancellationToken>()).Returns(true);

        var sut = new SeedDefaultPlansCommandHandler(subscriptionRepository);
        await sut.HandleAsync();

        await subscriptionRepository.DidNotReceiveWithAnyArgs().SeedPlansAsync(default!, default);
    }

    [Fact]
    public async Task HandleAsync_SeedsStarterGrowthAndPro_WhenNoPlansExist()
    {
        var subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        subscriptionRepository.HasAnyPlansAsync(Arg.Any<CancellationToken>()).Returns(false);

        var sut = new SeedDefaultPlansCommandHandler(subscriptionRepository);
        await sut.HandleAsync();

        await subscriptionRepository.Received(1).SeedPlansAsync(
            Arg.Is<IEnumerable<Plan>>(plans =>
                plans.Count() == 3 &&
                plans.Any(x => x.Code == "starter") &&
                plans.Any(x => x.Code == "growth") &&
                plans.Any(x => x.Code == "pro")),
            Arg.Any<CancellationToken>());
    }
}
