namespace ClinicSaas.Domain.Entities.Subscription;

using ClinicSaas.Domain.Common;

public sealed class Plan : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public bool IsWebsiteBuilderEnabled { get; set; } = true;
    public bool IsAiAssistantEnabled { get; set; }
    public bool IsSmartSchedulingEnabled { get; set; }
    public int MaxUsers { get; set; }
}
