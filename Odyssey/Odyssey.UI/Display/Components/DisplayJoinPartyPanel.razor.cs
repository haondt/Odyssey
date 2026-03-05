using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Display.Components
{
    public class DisplayJoinPartyModel
    {
        [Required]
        [Display(Name = "Join code")]
        public required string JoinCode { get; set; }

        [Required]
        [Display(Name = "Display name")]
        public required string DisplayName { get; set; }
    }
}
