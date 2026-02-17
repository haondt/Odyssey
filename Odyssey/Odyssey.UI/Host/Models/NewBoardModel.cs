using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Host.Models
{
    public class NewBoardModel
    {
        [Required(ErrorMessage = "Board name is required")]
        public required string Name { get; set; }

        [Required]
        public required string Game { get; set; }
    }
}
