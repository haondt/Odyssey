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

        public static class Hubs
        {
            public const string Index = "/hubs";
            public static class Host
            {
                public const string Index = $"{Hubs.Index}/host";
                public static class Browser
                {
                    public const string Index = $"{Hubs.Host.Index}/browser";
                }
            }
            public static class Display
            {
                public const string Index = $"{Hubs.Index}/display";
                public static class Browser
                {
                    public const string Index = $"{Hubs.Display.Index}/browser";
                }
            }
        }
        public static class Roles
        {
            public const string Index = "/roles";

        }
        public static class Fragments
        {
            public const string Index = $"/fragments";
            public class Websocket
            {
                public const string Index = $"{Fragments.Index}/websocket";
            }
            public static BottomSheetContent BottomSheetContentP(string role) => new(role);

            public class BottomSheetContent(string role)
            {
                public string IndexP = $"{Fragments.Index}/bottom-sheet-content/{role}";
            }
        }

        public static class Host
        {
            public const string Index = $"/{OdysseyRoles.Host}";
            public static class Party
            {
                public const string Index = $"{Host.Index}/party";
                public static class Reset
                {
                    public const string Index = $"{Party.Index}/reset";
                }
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
                public static Id IdP(string id) => new(id);
                public class Id(string id)
                {
                    public const string Index = $"{Host.Board.Index}/{{id}}";
                    public Reset ResetP => new($"{Host.Board.Index}/{id}");

                    public class Reset(string upperPath)
                    {
                        public const string Index = $"{Host.Board.Id.Index}/reset";
                        public string IndexP => $"{upperPath}/reset";
                    }

                    public Metadata MetadataP => new($"{Host.Board.Index}/{id}");
                    public class Metadata(string upperPath)
                    {
                        public const string Segment = "metadata";
                        public const string Index = $"{Host.Board.Id.Index}/{Segment}";
                        public string IndexP => $"{upperPath}/{Segment}";
                    }

                }
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
            public static class Party
            {
                public const string Index = $"{Display.Index}/party";
                public static class Join
                {
                    public const string Index = $"{Party.Index}/join";
                }
            }
        }

    }
}
