using System;

namespace LetsCodeItau.Dolly.Application.Interfaces.Providers;

public interface IDateTimeProvider
{
    DateTime DatetimeNow { get; }
    DateTime DateTimeUtcNow { get; }
}
