namespace Identity.Server.Application.Models;

public sealed record CachedSignInAttemptModel
{
    public ICollection<DateTimeOffset> AttemptedAt { get; set; }
    public int TotalAttempts { get; set; }

    public static CachedSignInAttemptModel Create(DateTimeOffset attemptedAt)
    {
        return new CachedSignInAttemptModel
        {
            AttemptedAt = new List<DateTimeOffset>
            {
                attemptedAt
            },
            TotalAttempts = 1
        };
    }
}
