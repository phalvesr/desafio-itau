using Dapper.Contrib.Extensions;
using Identity.Server.Domain.Enums;

namespace Identity.Server.Domain.Entities;

[Table("users_roles")]
public sealed class UsersRolesEntity
{
    public int UserRoleId { get; set; }
    public int UserAuthId { get; set; }
    public string GlobalId { get; set; }
    public string Role { get; set; }

    public static UsersRolesEntity Create(Guid globalId)
    {
        return new UsersRolesEntity
        {
            GlobalId = globalId.ToString(),
            Role = UserRolesEnum.Reader.StringRepresentation()
        };
    }
}
