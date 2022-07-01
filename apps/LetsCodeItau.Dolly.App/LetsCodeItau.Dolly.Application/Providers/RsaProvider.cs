using System.Security.Cryptography;
using LetsCodeItau.Dolly.Application.Interfaces.Providers;

namespace LetsCodeItau.Dolly.Application.Providers;

public class RsaProvider : IRsaProvider
{
    private readonly RSA _rsa = RSA.Create();
    public RSA Rsa => _rsa;
}
