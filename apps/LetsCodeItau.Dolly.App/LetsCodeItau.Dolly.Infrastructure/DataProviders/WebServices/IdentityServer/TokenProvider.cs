using System.Text;
using System.Text.Json;
using LetsCodeItau.Dolly.Application.Responses.IdentityServer;
using LetsCodeItau.Dolly.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.IdentityServer;

public class TokenProvider : BackgroundService, ITokenProvider
{
    private readonly HttpClient client;
    private readonly IConfiguration configuration;
    private AuthenticateAppResponseDto? lastResponse = null;

    public TokenProvider(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        this.client = httpClientFactory.CreateClient();
        this.configuration = configuration;

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        client.BaseAddress = new Uri(configuration["IdentityServer:BaseUrl"]);
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            await UpdateToken();
            await Task.Delay(TimeSpan.FromMinutes(3));
        }
    }

    private async Task UpdateToken()
    {
        var body = new AppSignInRequest(Guid.Parse(configuration["AppAuth:ClientKey"]),
            configuration["AppAuth:SecretKeySha256"]);

        var bodyAsJson = JsonSerializer.Serialize(body);

        var bodyContent = new StringContent(bodyAsJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(configuration["IdentityServer:TokenPath"], bodyContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(response.ReasonPhrase);
        }

        lastResponse = JsonSerializer.Deserialize<AuthenticateAppResponseDto>(response.Content.ReadAsStream());
    }

    public string Token => lastResponse?.AccessToken!;
}
