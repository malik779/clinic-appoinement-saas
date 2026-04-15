namespace ClinicSaas.Application.Abstractions;

public interface IAIService
{
    Task<string> ExecutePromptAsync(string prompt, CancellationToken cancellationToken = default);
}
