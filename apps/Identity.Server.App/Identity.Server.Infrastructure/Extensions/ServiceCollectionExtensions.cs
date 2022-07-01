using FluentValidation;
using Identity.Server.Application.Interfaces.Providers;
using Identity.Server.Application.Interfaces.UseCases;
using Identity.Server.Application.Providers;
using Identity.Server.Application.UseCases;
using Identity.Server.Domain.Gateways;
using Identity.Server.Infrastructure.DataProviders.Repositories;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers;
using Identity.Server.Infrastructure.DataProviders.Repositories.Wrappers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseServices(configuration);
            services.AddRedisServices(configuration);

            // Repositories  
            services.AddTransient<IDataMigrationRepository, DataMigrationRepository>();
            services.AddTransient<IUserAuthenticationRepository, UserAuthenticationRepository>();
            services.AddTransient<IUserCreationRepository, UserCreationRepository>();
            services.AddTransient<IAppAuthenticationRepository, AppAuthenticationRepository>();

            // Services  
            services.AddTransient<IDapperWrapper, DapperWrapper>();

            // UseCases 
            services.AddTransient<ISignInUserUseCaseAsync, SignInUserAsyncUseCase>();
            services.AddTransient<ICreateUserAsyncUseCase, CreateUserAsyncUseCase>();
            services.AddTransient<IApplyAttemptLimitUseCase, ApplyAttemptLimitUseCase>();
            services.AddTransient<ISignInAppAsyncUseCase, Application.UseCases.SignInAppAsyncUseCase>();
            services.AddTransient<IChangeUserRoleAsyncUseCase, ChangeUserRoleAsyncUseCase>();
            services.AddTransient<IChangeUserRoleRepository, ChangeUserRoleRepository>();

            // Providers
            services.AddSingleton<IRsaProvider, RsaProvider>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IJwtBearerTokenProvider, JwtBearerTokenProvider>();

            return services;
        }
    }
}
