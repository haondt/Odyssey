using Haondt.Core.Models;
using Haondt.Web.Core.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Odyssey.UI.Host.Components
{
    public class NewSessionModel
    {
        [Display(Name = "Name", Description = "If left empty, a session name will be automatically generated.")]
        public Optional<string> Name { get; set; }
        public required string GeneratedName { get; set; }

        [Required]
        [Display(Name = "Ephemeral", Description = "If checked, the session will be deleted after it ends.")]
        [ModelBinder(typeof(CheckboxModelBinder))]
        public required bool Ephemeral { get; set; }

        [Required]
        [Display(Name = "Board", Prompt = "Select board")]
        public required Guid Board { get; set; }

    }
}
