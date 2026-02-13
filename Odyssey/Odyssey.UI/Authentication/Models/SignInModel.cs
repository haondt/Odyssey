using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Authentication.Models
{
    public class SignInModel
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
