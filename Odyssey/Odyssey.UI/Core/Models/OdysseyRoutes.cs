namespace Odyssey.UI.Core.Models
{
    public static class OdysseyRoutes
    {
        public const string Index = "/";
        public const string Home = OdysseyRoutes.Roles.Index;
        public static class Auth
        {
            public const string Index = "/auth";
            public const string Register = $"{Auth.Index}/register";
            public const string SignIn = $"{Auth.Index}/sign-in";
            public const string SignOut = $"{Auth.Index}/sign-out";
        }
        public static class Roles
        {
            public const string Index = "/roles";

        }
        public static class Fragments
        {
            public const string Index = $"/fragments";
            public static class WebSocket
            {
                public const string Index = $"{Fragments.Index}/ws";
            }
        }
        public static class Host
        {
            public const string Index = $"/{OdysseyRoles.Host}";
            public static class Party
            {
                public const string Index = $"{Host.Index}/party";
            }
            public static class Sessions
            {
                public const string Index = $"{Host.Index}/sessions";
            }
            public static class Boards
            {
                public const string Index = $"{Host.Index}/boards";
                public static class New
                {
                    public const string Index = $"{Host.Boards.Index}/new";
                }
                public static class Search
                {
                    public const string Index = $"{Host.Boards.Index}/search";
                }
            }
            public static class Board
            {
                public const string Index = $"{Host.Index}/board";
            }
            public static class Soundboard
            {
                public const string Index = $"{Host.Index}/soundboard";
            }
            public static class Settings
            {
                public const string Index = $"{Host.Index}/settings";
            }
        }

        public static class Admin
        {
            public const string Index = $"/{OdysseyRoles.Admin}";
        }
        public static class Device
        {
            public const string Index = $"/{OdysseyRoles.Device}";
        }
        public static class Display
        {
            public const string Index = $"/{OdysseyRoles.Display}";
        }

    }
}
