using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Games.Domain.DebugGame.Models
{
    [GenerateSerializer]
    public class DebugGameGameState : IDataStorageData<DebugGameGameState>
    {
        [Id(0)]
        public List<int> Scores { get; set; } = [];
        [Id(1)]
        public int Round { get; set; } = 1;

        public static DebugGameGameState Factory() => new();
    }
}
