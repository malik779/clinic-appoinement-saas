using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClinicSaas.Infrastructure.Persistence;

public sealed class DateTimeUtcConverter : ValueConverter<DateTimeOffset?, DateTimeOffset?>
{
    public DateTimeUtcConverter()
        : base(
            value => value.HasValue ? value.Value.ToUniversalTime() : value,
            value => value.HasValue ? DateTime.SpecifyKind(value.Value.UtcDateTime, DateTimeKind.Utc) : value)
    {
    }
}
