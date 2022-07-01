using System.Security.Cryptography;
using Identity.Server.Application.Interfaces.Providers;

namespace Identity.Server.Application.Providers;

public class RsaProvider : IRsaProvider
{
    private readonly RSA _rsa = RSA.Create();
    public RSA Rsa => _rsa;
}
