using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Host.Components
{
    public class EditSessionMetadataPanelModel
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; } = "";
    }
}
