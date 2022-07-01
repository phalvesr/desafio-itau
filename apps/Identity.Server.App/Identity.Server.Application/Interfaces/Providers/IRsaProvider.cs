using System.Security.Cryptography;

namespace Identity.Server.Application.Interfaces.Providers;

public interface IRsaProvider
{
    RSA Rsa { get; }
}
