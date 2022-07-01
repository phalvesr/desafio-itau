namespace Identity.Server.Application.Interfaces.Providers;

public interface IDateTimeProvider
{
    DateTime DateTimeNow { get; }
    DateTimeOffset DateTimeOffsetNow { get; }
}
