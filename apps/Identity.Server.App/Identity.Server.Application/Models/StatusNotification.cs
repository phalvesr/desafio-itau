using Identity.Server.Application.Enums;

namespace Identity.Server.Application.Models;

public class StatusNotification<T>
{
    public T? Response { get; init; }
    public ProcessingStatusEnum Status { get; init; }


    public static StatusNotification<T> Create(T? response, ProcessingStatusEnum status)
    {
        return new StatusNotification<T>
        {
            Response = response,
            Status = status
        };
    }
}
