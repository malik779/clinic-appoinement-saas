using ClinicSaas.Application.Abstractions;

namespace ClinicSaas.Infrastructure.Auth;

public sealed class NoOpAiService : IAIService
{
    public Task<string> ExecutePromptAsync(string prompt, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("AI provider is not configured yet.");
    }
}
