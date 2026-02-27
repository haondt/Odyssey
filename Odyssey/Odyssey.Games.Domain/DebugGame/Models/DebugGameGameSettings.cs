using Odyssey.Domain.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Games.Domain.DebugGame.Models
{
    [GenerateSerializer]
    public class DebugGameGameSettings : GameSettings, IDataStorageData<DebugGameGameSettings>
    {
        [Id(0)]
        public required string SettingOne { get; set; }
        [Id(1)]
        public required bool SettingTwo { get; set; }
        [Id(2)]
        public override required string DisplayName { get; set; }

        public static DebugGameGameSettings Factory() => new()
        {
            SettingOne = "Value 1",
            SettingTwo = true,
            DisplayName = "Debug"
        };
    }
}
