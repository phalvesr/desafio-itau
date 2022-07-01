using System.Net;

namespace LetsCodeItau.Dolly.Application.Core;

public class Result<T> where T : class
{
    public T Response { get; private set; } = default!;
    public bool Succeeded { get; private set; } = default!;
    public HttpStatusCode StatusCode { get; private set; } = default!;
    public string Error { get; private set; } = default!;

    public static Result<T> Create(T response, bool succeeded, HttpStatusCode statusCode, string error)
    {
        return new Result<T>
        {
            Response = response,
            Succeeded = succeeded,
            Error = error,
            StatusCode = statusCode
        };
    }
}
