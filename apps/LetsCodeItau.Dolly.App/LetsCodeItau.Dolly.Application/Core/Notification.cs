using LetsCodeItau.Dolly.Application.Enums;

namespace LetsCodeItau.Dolly.Application.Core;

public class Notification<T>
{
    public T? Data { get; private set; }
    public ProcessingStatusEnum Status { get; private set; }
    public string[] Errors { get; private set; } = Array.Empty<string>();

    public static Notification<T> Create(T data, ProcessingStatusEnum status, params string[] errors)
    {
        return new Notification<T>
        {
            Data = data,
            Errors = errors,
            Status = status
        };
    }
}
