using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Authentication.Models.Authentication
{
    public class SignInModel
    {
        [Required]
        [Display(Name = "Username")]
        public required string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }
    }
}
