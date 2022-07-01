using LetsCodeItau.Dolly.Application.Requests;
using LetsCodeItau.Dolly.Application.Responses.IdentityServer;
using LetsCodeItau.Dolly.Infrastructure.Models;
using Refit;

namespace LetsCodeItau.Dolly.Infrastructure.DataProviders.WebServices.IdentityServer;

public interface IIdentityServerApi
{
    [Post("/auth/users")]
    Task<IApiResponse<AuthenticateUserResponseDto>> AuthenticateUserAsync([Body] UserSignInRequest body);

    [Patch("/roles")]
    Task<IApiResponse<ChangeUserRoleResponseDto>> ChangeUserRoleAsync([Body] ChangeUserRoleRequest body);

    [Post("/users")]
    Task<IApiResponse<CreateUserResponseDto>> CreateUserAsync([Body] CreateUserRequest body);
}
