namespace Identity.Server.Domain.Enums;

public enum ApplicationRolesEnum
{
    Application = 1
}

public static class ApplicationRolesEnumExtensions
{
    public static string StringRepresentation(this ApplicationRolesEnum value)
    {
        return value switch
        {
            ApplicationRolesEnum.Application => "APPLICATION",
            _ => throw new ArgumentException("User role not known", nameof(value))
        };
    }
}
