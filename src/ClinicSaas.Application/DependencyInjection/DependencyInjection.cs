using ClinicSaas.Application.Auth.Commands;
using ClinicSaas.Application.Subscriptions.Commands;
using ClinicSaas.Application.Tenants.Commands;
using ClinicSaas.Application.Tenants.Queries;
using ClinicSaas.Application.Websites.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicSaas.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterTenantCommandHandler>();
        services.AddScoped<RegisterUserCommandHandler>();
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<GetTenantInfoQueryHandler>();
        services.AddScoped<SeedDefaultPlansCommandHandler>();
        services.AddScoped<EnsureDefaultWebsiteCommandHandler>();

        return services;
    }
}
