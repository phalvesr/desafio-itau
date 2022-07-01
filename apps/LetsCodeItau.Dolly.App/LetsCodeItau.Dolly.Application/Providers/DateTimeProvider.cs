using LetsCodeItau.Dolly.Application.Interfaces.Providers;

namespace LetsCodeItau.Dolly.Application.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DatetimeNow => DateTime.Now;

    public DateTime DateTimeUtcNow => DateTime.UtcNow;
}
