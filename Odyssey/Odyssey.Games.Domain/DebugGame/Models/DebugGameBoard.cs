using Haondt.Web.Core.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Odyssey.GrainInterfaces.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace Odyssey.Games.Domain.DebugGame.Models
{

    [GenerateSerializer]
    public class DebugGameBoard : IDataStorageData<DebugGameBoard>
    {
        public static DebugGameBoard Factory() => new DebugGameBoard()
        {
            Section = new()
            {
                SomeString = "Some value",
                SomeOtherString = "Some other value"
            },
            SomeCheckbox = true,
        };

        [Id(0)]
        [Display(Name = "Some section")]
        public required DebugGameBoardSection Section { get; set; }

        [Display(Name = "Some checkbox value", Description = "This is a description for the checkbox value. Use it as you must.")]
        [ModelBinder(typeof(CheckboxModelBinder))]
        [Id(1)]
        public bool SomeCheckbox { get; set; }

    }

    [GenerateSerializer]
    public class DebugGameBoardSection
    {
        [Display(Name = "Some string", Description = "This is some string.")]
        [Required]
        [Id(0)]
        public required string SomeString { get; set; }

        [Display(Name = "Some other string")]
        [Id(1)]
        public string? SomeOtherString { get; set; }
    }
}
