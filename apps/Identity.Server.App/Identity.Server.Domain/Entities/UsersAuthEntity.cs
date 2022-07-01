using Dapper.Contrib.Extensions;

namespace Identity.Server.Domain.Entities;

[Table("users_auth")]
public sealed class UsersAuthEntity
{
    [Key]
    public int UserAuthId { get; set; }
    public string GlobalId { get; set; }
    public string Username { get; set; }
    public string EncryptedPassword { get; set; }

    public static UsersAuthEntity Create(Guid globalId, string username, string encriptedPassword)
    {
        return new UsersAuthEntity
        {
            EncryptedPassword = encriptedPassword,
            GlobalId = globalId.ToString(),
            Username = username
        };
    }
}
