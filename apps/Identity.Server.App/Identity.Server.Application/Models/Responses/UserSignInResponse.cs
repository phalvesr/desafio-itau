namespace Identity.Server.Application.Models.Responses;

public sealed class UserSignInResponse
{

    public string AccessToken { get; set; }
    public DateTimeOffset UnixTimeExpiresAt { get; set; }
    public Guid GlobalId { get; set; }

    public UserSignInResponse(string accessToken, DateTimeOffset unixTimeExpiresAt, string globalId)
    {
        AccessToken = accessToken;
        UnixTimeExpiresAt = unixTimeExpiresAt;
        GlobalId = Guid.Parse(globalId);
    }
}
