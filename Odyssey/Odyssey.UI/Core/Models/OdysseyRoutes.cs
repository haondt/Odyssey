namespace Odyssey.UI.Core.Models
{
    public static class OdysseyRoutes
    {
        public const string Index = "/";
        public const string Home = OdysseyRoutes.Roles.Index;
        public static class Auth
        {
            public const string Index = $"/auth";
            public const string Register = $"{Auth.Index}/register";
            public const string SignIn = $"{Auth.Index}/sign-in";
            public const string SignOut = $"{Auth.Index}/sign-out";
        }
        public static class Roles
        {
            public const string Index = $"/roles";
            public const string Host = $"{Roles.Index}/host";
            public const string Admin = $"{Roles.Index}/admin";
            public const string Device = $"{Roles.Index}/device";
            public const string Display = $"{Roles.Index}/display";
        }
    }
}
