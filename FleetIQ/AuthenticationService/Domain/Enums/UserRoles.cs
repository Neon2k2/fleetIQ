namespace AuthenticationService.Domain.Enums;

public static class UserRoles
{
    public const string Owner = "Owner";
    public const string Admin = "Admin";
    public const string Driver = "Driver";

    public static readonly IReadOnlyList<string> All = new[] { Owner, Admin, Driver };

    public static bool IsValid(string role)
    {
        return All.Contains(role);
    }
}
