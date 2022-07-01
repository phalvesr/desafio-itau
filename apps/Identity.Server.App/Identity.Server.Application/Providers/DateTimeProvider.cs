using Identity.Server.Application.Interfaces.Providers;

namespace Identity.Server.Application.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeNow => DateTime.Now;

    public DateTimeOffset DateTimeOffsetNow => DateTimeOffset.Now;
}
