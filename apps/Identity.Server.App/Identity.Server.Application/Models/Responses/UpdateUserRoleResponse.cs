namespace Identity.Server.Application.Models.Responses;

public class UpdateUserRoleResponse
{

    public Guid GlobalId { get; set; }
    public string Role { get; set; }

    public UpdateUserRoleResponse(Guid globalId, string role)
    {
        GlobalId = globalId;
        Role = role;
    }
}
