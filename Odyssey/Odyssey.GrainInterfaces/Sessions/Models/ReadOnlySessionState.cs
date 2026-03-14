namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    [Immutable]
    public record ReadOnlySessionState
    {
        [Id(0)]
        public required int Version { get; init; }
        [Id(1)]
        public required IReadOnlyList<ReadOnlySessionPlayer> Players { get; init; }
    }

    [GenerateSerializer]
    [Immutable]
    public class ReadOnlySessionPlayer
    {
        [Id(0)]
        public required string Name { get; init; }
        [Id(1)]
        public required IReadOnlyCollection<Guid> Devices { get; init; }
    }
}
