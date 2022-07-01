using System.Security.Cryptography;

namespace LetsCodeItau.Dolly.Application.Interfaces.Providers;

public interface IRsaProvider
{
    public RSA Rsa { get; }
}
