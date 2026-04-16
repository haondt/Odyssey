using System.ComponentModel.DataAnnotations;
using Haondt.Web.Core.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace Odyssey.UI.Host.Components.Lobby
{
    public class HostLobbyPlayerData
    {
        [Display(Name = "Name")]
        [Required]
        public required string Name { get; set; }

        [Display(Name = "Assign devices", Prompt = "Select device")]
        public HashSet<Guid> DeviceIds { get; set; } = [];

        [Display(Name = "Ready")]
        [ModelBinder(typeof(CheckboxModelBinder))]
        public bool Ready { get; set; }
    }

}
