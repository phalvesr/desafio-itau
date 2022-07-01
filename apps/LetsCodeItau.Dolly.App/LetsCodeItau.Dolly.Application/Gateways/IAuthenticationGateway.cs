using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Requests;
using LetsCodeItau.Dolly.Application.Responses.IdentityServer;

namespace LetsCodeItau.Dolly.Application.Gateways;

public interface IAuthenticationGateway
{
    Task<Result<AuthenticateUserResponseDto>> AuthenticateUserAsync(UserSignInRequest request);
    Task<Result<ChangeUserRoleResponseDto>> ChangeUserRoleAsync(ChangeUserRoleRequest request);
    Task<Result<CreateUserResponseDto>> CreateUserAsync(CreateUserRequest request);
}
