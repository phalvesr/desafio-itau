using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Models;
using Identity.Server.Application.Models.Requests;
using Identity.Server.Domain.Gateways;
using Microsoft.Extensions.Configuration;

namespace Identity.Server.Application.UseCases;

public class ApplyAttemptLimitUseCase : IApplyAttemptLimitUseCase
{
    private readonly ICacheRepository<CachedSignInAttemptModel> cacheRepository;
    private readonly IConfiguration configuration;

    public ApplyAttemptLimitUseCase(
        ICacheRepository<CachedSignInAttemptModel> cacheRepository,
        IConfiguration configuration)
    {
        this.cacheRepository = cacheRepository;
        this.configuration = configuration;
    }

    public async Task<bool> ExecuteAsync(UserSignInRequest request)
    {
        var cached = await cacheRepository.GetEntryOrDefaultAsync(request.Username);

        if (cached is null)
        {
            return false;
        }

        return cached.TotalAttempts > int.Parse(configuration["AttemptLimit"]) - 1;
    }
}
