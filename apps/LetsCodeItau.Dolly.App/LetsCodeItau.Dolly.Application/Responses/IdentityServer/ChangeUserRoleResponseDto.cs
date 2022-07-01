namespace LetsCodeItau.Dolly.Application.Responses.IdentityServer;

public class ChangeUserRoleResponseDto
{
    public Guid GlobalId { get; set; }
    public string Role { get; set; } = default!;
}
