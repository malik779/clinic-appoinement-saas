using ClinicSaas.Application.Subscriptions.Commands;
using ClinicSaas.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicSaas.Infrastructure.Persistence;

public static class DefaultSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        foreach (var role in ApplicationRole.DefaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole(role));
            }
        }

        var seedDefaultPlansHandler = scope.ServiceProvider.GetRequiredService<SeedDefaultPlansCommandHandler>();
        await seedDefaultPlansHandler.HandleAsync();
    }
}
