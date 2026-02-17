using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Authentication.Models.Authentication
{
    public class RegisterModel
    {
        [Required]
        public required string InviteCode { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string ConfirmPassword { get; set; }
    }
}
