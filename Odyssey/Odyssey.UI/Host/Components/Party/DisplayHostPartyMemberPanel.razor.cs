using System.ComponentModel.DataAnnotations;
using Haondt.Web.Core.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.UI.Host.Components
{
    public class DisplayHostPartyMemberPanelModel
    {
        public static DisplayHostPartyMemberPanelModel Create(Guid id, HostDisplayData Data) => new()
        {
            Id = id,
            PlaySounds = Data.PlaySounds,
            ReflectSoundboard = Data.ReflectSoundBoard
        };

        public HostDisplayData Apply(HostDisplayData data) => data with
        {
            PlaySounds = PlaySounds,
            ReflectSoundBoard = ReflectSoundboard
        };

        [Required]
        public required Guid Id { get; set; }

        [Display(Name = "Play sounds")]
        [ModelBinder(typeof(CheckboxModelBinder))]
        public required bool PlaySounds { get; set; }

        [Display(Name = "Play sounds")]
        [ModelBinder(typeof(CheckboxModelBinder))]
        public required bool ReflectSoundboard { get; set; }
    }
}
