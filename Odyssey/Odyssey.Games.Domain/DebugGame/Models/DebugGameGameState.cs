using Odyssey.GrainInterfaces.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace Odyssey.Games.Domain.DebugGame.Models
{
    [GenerateSerializer]
    public class DebugGameGameState : IDataStorageData<DebugGameGameState>
    {
        [Id(0)]
        [Display(Name = "Scores")]
        public List<int> Scores { get; set; } = [];
        [Id(1)]
        [Display(Name = "Current round")]
        [Range(1, 99)]
        public int Round { get; set; } = 1;

        public static DebugGameGameState Factory() => new();
    }
}
