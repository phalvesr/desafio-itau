using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.Providers;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Mappers;
using LetsCodeItau.Dolly.Application.Providers;
using LetsCodeItau.Dolly.Application.UseCases;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.Database;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.IdentityServer;
using LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.Omdb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseServices(configuration);

        // Gateways 
        services.AddTransient<IAuthenticationGateway, AuthenticationGateway>();
        services.AddTransient<IOmdbGateway, OmdbGateway>();

        // Providers
        services.AddSingleton<IRsaProvider, RsaProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // Usecases
        services.AddTransient<ICreateUserAsyncUseCase, CreateUserAsyncUseCase>();
        services.AddTransient<ILogUserInAsyncUseCase, LogUserInAsyncUseCase>();
        services.AddTransient<ISearchMovieByTitleAsyncUseCase, SearchMovieByTitleAsyncUseCase>();
        services.AddTransient<IPostCommentAsyncUseCase, PostCommentAsyncUseCase>();
        services.AddTransient<IDeleteCommentAsyncUseCase, DeleteCommentAsyncUseCase>();
        services.AddTransient<IReactToCommentAsyncUseCase, ReactToCommentAsyncUseCase>();
        services.AddTransient<ITurnUserModeratorAsyncUseCase, TurnUserModeratorAsyncUseCase>();
        services.AddTransient<IRateMovieAsyncUseCase, RateMovieAsyncUseCase>();
        services.AddTransient<ICreateRatingFromImdbIdAsyncUseCase, CreateRatingFromImdbId>();
        services.AddTransient<IFindUserDataAsyncUseCase, FindUserDataAsyncUseCase>();
        services.AddTransient<IGetCommentsAsyncUseCase, GetCommentsAsyncUseCase>();

        services
            .AddEvents()
            .AddApplicationToken()
            .AddSerilog();

        // Libs
        services.AddRefit(configuration);
        services.AddAutoMapper(typeof(IAutomapperMarker));

        return services;
    }
}
