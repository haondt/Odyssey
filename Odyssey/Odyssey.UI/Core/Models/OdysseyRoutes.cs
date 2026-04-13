using Odyssey.GrainInterfaces.Sessions.Models;

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
            public static class Device
            {
                public const string Index = $"{Hubs.Index}/device";
                public static class Browser
                {
                    public const string Index = $"{Hubs.Device.Index}/browser";
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


                public class Members
                {
                    public const string Index = $"{Party.Index}/members";
                    public static Id IdP(PartyMemberId memberId) => new(memberId);
                    public class Id(PartyMemberId id)
                    {
                        public const string Index = $"{Party.Members.Index}/{{id}}";
                        public string IndexP = $"{Party.Members.Index}/{id}";

                        public Display DisplayP => new(IndexP);
                        public class Display(string upperPath)
                        {
                            public const string Segment = "display";
                            public const string Index = $"{Party.Members.Id.Index}/{Segment}";
                            public string IndexP => $"{upperPath}/{Segment}";
                        }

                        public Device DeviceP => new(IndexP);
                        public class Device(string upperPath)
                        {
                            public const string Segment = "device";
                            public const string Index = $"{Party.Members.Id.Index}/{Segment}";
                            public string IndexP => $"{upperPath}/{Segment}";
                        }
                    }
                }

                public static class Session
                {
                    public const string Index = $"{Party.Index}/session";
                }
            }
            public static class Sessions
            {
                public const string Index = $"{Host.Index}/sessions";
                public static class New
                {
                    public const string Index = $"{Host.Sessions.Index}/new";
                }
                public static class Search
                {
                    public const string Index = $"{Host.Sessions.Index}/search";
                }
            }
            public static class Session
            {
                public const string Index = $"{Host.Index}/session";
                public static Id IdP(Guid id) => new(id);
                public class Id(Guid id)
                {
                    public const string Index = $"{Host.Session.Index}/{{id}}";
                    public string IndexP => $"{Host.Session.Index}/{id}";

                    public Metadata MetadataP => new($"{Host.Session.Index}/{id}");
                    public class Metadata(string upperPath)
                    {
                        public const string Segment = "metadata";
                        public const string Index = $"{Host.Session.Id.Index}/{Segment}";
                        public string IndexP => $"{upperPath}/{Segment}";
                    }
                    public Archive ArchiveP => new($"{Host.Session.Index}/{id}");
                    public class Archive(string upperPath)
                    {
                        public const string Segment = "archive";
                        public const string Index = $"{Host.Session.Id.Index}/{Segment}";
                        public string IndexP => $"{upperPath}/{Segment}";
                    }

                    public Unarchive UnarchiveP => new($"{Host.Session.Index}/{id}");
                    public class Unarchive(string upperPath)
                    {
                        public const string Segment = "unarchive";
                        public const string Index = $"{Host.Session.Id.Index}/{Segment}";
                        public string IndexP => $"{upperPath}/{Segment}";
                    }

                    public GameState GameStateP => new($"{Host.Session.Index}/{id}");
                    public class GameState(string upperPath)
                    {
                        public const string Segment = "game-state";
                        public const string Index = $"{Host.Session.Id.Index}/{Segment}";
                        public string IndexP => $"{upperPath}/{Segment}";

                        public Reset ResetP => new($"{upperPath}/{Segment}");

                        public class Reset(string upperPath)
                        {
                            public const string Index = $"{Host.Session.Id.GameState.Index}/reset";
                            public string IndexP => $"{upperPath}/reset";
                        }

                        public Raw RawP => new($"{upperPath}/{Segment}");
                        public class Raw(string upperPath)
                        {
                            public const string Segment = "raw";
                            public const string Index = $"{Host.Session.Id.GameState.Index}/{Segment}";
                            public string IndexP => $"{upperPath}/{Segment}";
                            public Reset ResetP => new($"{upperPath}/{Segment}");

                            public class Reset(string upperPath)
                            {
                                public const string Index = $"{Host.Session.Id.GameState.Raw.Index}/reset";
                                public string IndexP => $"{upperPath}/reset";
                            }
                        }
                    }

                    public Settings SettingsP => new($"{Host.Session.Index}/{id}");
                    public class Settings(string upperPath)
                    {
                        public const string Segment = "settings";
                        public const string Index = $"{Host.Session.Id.Index}/{Segment}";
                        public string IndexP => $"{upperPath}/{Segment}";
                    }

                    public Launch LaunchP => new($"{Host.Session.Index}/{id}");
                    public class Launch(string upperPath)
                    {
                        public const string Segment = "launch";
                        public const string Index = $"{Host.Session.Id.Index}/{Segment}";
                        public string IndexP => $"{upperPath}/{Segment}";
                    }

                    public Lobby LobbyP => new($"{Host.Session.Index}/{id}");
                    public class Lobby(string upperPath)
                    {
                        public const string Segment = "lobby";
                        public const string Index = $"{Host.Session.Id.Index}/{Segment}";
                        public string IndexP => $"{upperPath}/{Segment}";
                    }
                }
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
                public static class Suggest
                {
                    public const string Index = $"{Host.Boards.Index}/suggest";
                }
            }
            public static class Board
            {
                public const string Index = $"{Host.Index}/board";
                public static Id IdP(Guid id) => new(id);
                public class Id(Guid id)
                {
                    public const string Index = $"{Host.Board.Index}/{{id}}";
                    public string IndexP = $"{Host.Board.Index}/{id}";
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
            public static class Party
            {
                public const string Index = $"{Device.Index}/party";
                public static class Join
                {
                    public const string Index = $"{Party.Index}/join";
                }

                public static class Leave
                {
                    public const string Index = $"{Party.Index}/leave";
                }
            }
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

                public static class Leave
                {
                    public const string Index = $"{Party.Index}/leave";
                }
            }
        }

    }
}
