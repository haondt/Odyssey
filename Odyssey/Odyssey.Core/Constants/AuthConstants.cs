namespace Odyssey.Core.Constants
{
    public class AuthConstants
    {
        public static readonly IReadOnlyList<string> Roles =
        [
            AdminRole,
            SuperadminRole
        ];

        public const string AdminRole = "Admin";
        public const string SuperadminRole = "Superadmin";
        public const string AllowedUsernameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

    }
}
