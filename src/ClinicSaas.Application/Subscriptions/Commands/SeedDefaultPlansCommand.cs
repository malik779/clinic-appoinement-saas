namespace ClinicSaas.Application.Subscriptions.Commands;

using ClinicSaas.Application.Abstractions;
using System.Threading;
using System.Threading.Tasks;

public sealed class SeedDefaultPlansCommand;

public sealed class SeedDefaultPlansCommandHandler(ISubscriptionRepository subscriptionRepository)
{
    public async Task HandleAsync(CancellationToken cancellationToken = default)
    {
        if (await subscriptionRepository.HasAnyPlansAsync(cancellationToken))
        {
            return;
        }

        var plans =
            new[]
            {
                new Domain.Entities.Subscription.Plan
                {
                    Name = "Starter",
                    Code = "starter",
                    MonthlyPrice = 29,
                    IsWebsiteBuilderEnabled = true,
                    IsAiAssistantEnabled = false,
                    IsSmartSchedulingEnabled = false,
                    MaxUsers = 3
                },
                new Domain.Entities.Subscription.Plan
                {
                    Name = "Growth",
                    Code = "growth",
                    MonthlyPrice = 79,
                    IsWebsiteBuilderEnabled = true,
                    IsAiAssistantEnabled = true,
                    IsSmartSchedulingEnabled = false,
                    MaxUsers = 15
                },
                new Domain.Entities.Subscription.Plan
                {
                    Name = "Pro",
                    Code = "pro",
                    MonthlyPrice = 199,
                    IsWebsiteBuilderEnabled = true,
                    IsAiAssistantEnabled = true,
                    IsSmartSchedulingEnabled = true,
                    MaxUsers = 100
                }
            };

        await subscriptionRepository.SeedPlansAsync(plans, cancellationToken);
    }
}

