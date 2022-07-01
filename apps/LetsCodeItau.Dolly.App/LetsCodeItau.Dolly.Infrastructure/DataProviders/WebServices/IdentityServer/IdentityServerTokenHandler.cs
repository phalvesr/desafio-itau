using System.Net.Http.Headers;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.IdentityServer;

public class IdentityServerTokenHandler : DelegatingHandler
{
    private readonly ITokenProvider tokenProvider;

    public IdentityServerTokenHandler(ITokenProvider tokenProvider)
    {
        this.tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = tokenProvider.Token;

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
