namespace ClinicSaas.Api.Contracts;

public sealed record ApiResponse<T>(bool Success, T? Data, IReadOnlyCollection<string> Errors);
