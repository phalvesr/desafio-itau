using System.Net;
using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Requests;
using LetsCodeItau.Dolly.Application.Responses.IdentityServer;
using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.IdentityServer;

public class AuthenticationGateway : IAuthenticationGateway
{
    private readonly IIdentityServerApi identityServerApi;

    public AuthenticationGateway(IIdentityServerApi identityServerApi)
    {
        this.identityServerApi = identityServerApi;
    }

    public async Task<Result<AuthenticateUserResponseDto>> AuthenticateUserAsync(UserSignInRequest request)
    {
        try
        {
            var response = await identityServerApi.AuthenticateUserAsync(request);

            return GetResult(response);
        }
        catch (HttpRequestException)
        {
            return GetResultForExternalDependencyIntegrationFailure<AuthenticateUserResponseDto>();
        }
        catch (Exception)
        {
            return GetResultForInternalServerError<AuthenticateUserResponseDto>();
        }
    }

    public async Task<Result<ChangeUserRoleResponseDto>> ChangeUserRoleAsync(ChangeUserRoleRequest request)
    {
        try
        {
            var response = await identityServerApi.ChangeUserRoleAsync(request);

            return GetResult(response);
        }
        catch (HttpRequestException)
        {
            return GetResultForExternalDependencyIntegrationFailure<ChangeUserRoleResponseDto>();
        }
        catch (Exception)
        {
            return GetResultForInternalServerError<ChangeUserRoleResponseDto>();
        }
    }

    public async Task<Result<CreateUserResponseDto>> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var response = await identityServerApi.CreateUserAsync(request);

            return GetResult(response);
        }
        catch (HttpRequestException)
        {
            return GetResultForExternalDependencyIntegrationFailure<CreateUserResponseDto>();
        }
        catch (Exception)
        {
            return GetResultForInternalServerError<CreateUserResponseDto>();
        }
    }

    private Result<T> GetResult<T>(IApiResponse<T> response) where T : class
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = response.Error?.Content;

            return Result<T>.Create(null!, false, response.StatusCode, error!);
        }

        var content = response.Content;

        return Result<T>.Create(content!, true, response.StatusCode, null!);
    }

    private Result<T> GetResultForExternalDependencyIntegrationFailure<T>() where T : class
    {
        return Result<T>.Create(null!, false, HttpStatusCode.BadGateway, "Failed while connectiong to external dependency.");
    }

    private Result<T> GetResultForInternalServerError<T>() where T : class
    {
        return Result<T>.Create(null!, false, HttpStatusCode.InternalServerError, string.Empty);
    }
}
