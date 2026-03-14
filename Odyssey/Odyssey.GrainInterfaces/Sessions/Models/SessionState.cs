using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public record SessionState<TBoard> : IDataStorageData<SessionState<TBoard>> where TBoard : IDataStorageData<TBoard>
    {
        [Id(0)]
        public required TBoard Board { get; set; }

        [Id(1)]
        public required List<SessionPlayer> Players { get; set; }

        public static SessionState<TBoard> Factory() => new()
        {
            Board = TBoard.Factory(),
            Players = [],
        };
    }

    [GenerateSerializer]
    public class SessionPlayer
    {
        [Id(0)]
        public required string Name { get; set; }
        [Id(1)]
        public HashSet<Guid> Devices { get; set; } = [];
    }
}
