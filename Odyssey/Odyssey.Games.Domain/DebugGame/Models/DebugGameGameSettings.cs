using Odyssey.Domain.Core.Models;

namespace Odyssey.Games.Domain.DebugGame.Models
{
    [GenerateSerializer]
    public class DebugGameGameSettings : GameSettings
    {
        [Id(0)]
        public string SettingOne { get; set; } = "Value 1";
        [Id(1)]
        public bool SettingTwo { get; set; } = true;
        [Id(2)]
        public override string DisplayName { get; set; } = "Debug";
    }
}
