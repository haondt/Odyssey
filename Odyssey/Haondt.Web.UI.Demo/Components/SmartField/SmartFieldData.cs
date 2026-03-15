using System.ComponentModel.DataAnnotations;

namespace Haondt.Web.UI.Demo.Components.SmartField
{
    public class SmartFieldData
    {
        [Required]
        public required Car Car { get; set; }

        [Required]
        [Display(Name = "String", Description = "This value is required. Try submitting the form with an empty value.")]
        public required string String { get; set; }

        [Display(Name = "Optional string", Description = "This value is optional", Prompt = "A customizable prompt")]
        // TODO: this should work with Optional<T>
        public string? OptionalString { get; set; }
    }

    public class Car
    {
        public List<Wheel> Wheels { get; set; } = [];
        [Display(Name = "VIN")]
        public required string Vin { get; set; }
        [Display(Name = "Radio Presets")]
        public List<double> RadioPresets { get; set; } = [];
    }

    public class Wheel
    {
        [Required]
        [Display(Name = "Wheel Radii")]
        public required int Radius { get; set; }

        [Required]
        [Display(Name = "Wheel Wear Levels", Description = "Smart field list can group the items and set display and title for the group as a whole.")]
        public required string WearLevel { get; set; }
        public Tire Tire { get; set; } = new();
    }

    public class Tire
    {
        [Display(Name = "Tire Seasonality", Description = "Smart field list even works on lists of non-primitives!")]
        public string? Seasonality { get; set; }
    }
}
