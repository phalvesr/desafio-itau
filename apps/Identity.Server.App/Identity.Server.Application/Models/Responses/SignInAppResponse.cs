namespace Identity.Server.Application.Models.Responses;

public sealed class SignInAppResponse
{

    public string AccessToken { get; set; }
    public DateTimeOffset UnixExpirationTime { get; set; }

    public SignInAppResponse(string accessToken, DateTimeOffset unixExpirationTime)
    {
        AccessToken = accessToken;
        UnixExpirationTime = unixExpirationTime;
    }
}
