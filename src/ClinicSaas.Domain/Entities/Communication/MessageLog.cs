using ClinicSaas.Domain.Common;

namespace ClinicSaas.Domain.Entities.Communication;

public sealed class MessageLog : TenantEntity
{
    public string Channel { get; set; } = string.Empty;

    public string Recipient { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public DateTimeOffset SentAtUtc { get; set; }

    public bool Successful { get; set; }
}
