using System.ComponentModel.DataAnnotations;

namespace Odyssey.Games.Domain.DebugGame.Models
{
    [GenerateSerializer]
    public class DebugGameBoard
    {
        [Id(0)]
        public DebugGameBoardSection Section { get; set; } = new();

        [Display(Name = "Some checkbox value", Description = "This is a description for the checkbox value. Use it as you must.")]
        [Id(1)]
        public bool SomeCheckbox { get; set; } = true;
    }

    [GenerateSerializer]
    public class DebugGameBoardSection
    {
        [Display(Name = "Some string")]
        [Required]
        [Id(0)]
        public string SomeString { get; set; } = "Some Value";

        [Display(Name = "Some other string")]
        [Id(1)]
        public string SomeOtherString { get; set; } = "Some other Value";
    }
}
