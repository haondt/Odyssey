using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Device.Components
{
    public class DeviceJoinPartyModel
    {
        [Required]
        [Display(Name = "Join code")]
        public required string JoinCode { get; set; }

        [Required]
        [Display(Name = "Device name", Prompt = "Sofia's iPad")]
        public required string DeviceName { get; set; }
    }
}
