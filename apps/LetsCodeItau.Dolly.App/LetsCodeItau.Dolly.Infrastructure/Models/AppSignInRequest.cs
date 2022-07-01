namespace LetsCodeItau.Dolly.Infrastructure.Models;

public sealed record AppSignInRequest
(
    Guid ClientKey,
    string SecretKeySha256
);
