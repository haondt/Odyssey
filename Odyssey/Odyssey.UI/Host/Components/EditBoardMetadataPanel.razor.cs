using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Host.Components
{
    public class EditBoardMetadataPanelModel
    {
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; } = "";
    }
}
