namespace Identity.Server.Application.Models.Requests;

public sealed record AppSignInRequest
(
    Guid ClientKey,
    string SecretKeySha256
);
