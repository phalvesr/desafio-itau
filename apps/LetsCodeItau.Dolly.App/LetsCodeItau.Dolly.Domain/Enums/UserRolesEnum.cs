namespace LetsCodeItau.Dolly.Domain.Enums;

public enum UserRolesEnum
{
    Reader = 1,
    Basic = 2,
    Advanced = 3,
    Moderator = 4
}

public static class UserRolesEnumExtensions
{
    public static string StringRepresentation(this UserRolesEnum value)
    {
        return value switch
        {
            UserRolesEnum.Reader => "READER",
            UserRolesEnum.Basic => "BASIC",
            UserRolesEnum.Advanced => "ADVANCED",
            UserRolesEnum.Moderator => "MODERATOR",
            _ => throw new ArgumentException("User role not known", nameof(value))
        };
    }
}
