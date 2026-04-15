using ClinicSaas.Domain.Entities.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicSaas.Infrastructure.Persistence.Configurations;

public sealed class MessageLogConfiguration : IEntityTypeConfiguration<MessageLog>
{
    public void Configure(EntityTypeBuilder<MessageLog> builder)
    {
        builder.ToTable("MessageLogs", "communication");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Channel).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Recipient).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Payload).HasMaxLength(4000).IsRequired();
        builder.Property(x => x.SentAtUtc).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.SentAtUtc });
    }
}
