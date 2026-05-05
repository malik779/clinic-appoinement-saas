using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClinicSaas.Api.Configuration;

public static class CorsConfiguration
{
    public const string FrontendPolicyName = "Frontend";
    private const string DefaultDevelopmentOrigin = "http://localhost:5173";

    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var configuredOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        var allowedOrigins = configuredOrigins
            .Where(static origin => !string.IsNullOrWhiteSpace(origin))
            .Select(static origin => origin.Trim().TrimEnd('/'))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (allowedOrigins.Length == 0 && (environment.IsDevelopment() || environment.IsEnvironment("Testing")))
        {
            allowedOrigins = [DefaultDevelopmentOrigin];
        }

        services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicyName, policy =>
            {
                if (allowedOrigins.Length == 0)
                {
                    return;
                }

                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
