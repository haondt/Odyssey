using System.ComponentModel.DataAnnotations;
using Haondt.Web.Core.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Odyssey.GrainInterfaces.Sessions.Models;
using HostSettingsData = Odyssey.GrainInterfaces.Sessions.Models.HostSettings;

namespace Odyssey.UI.Host.Components.Settings
{
    public class HostSettingsModel
    {
        public static HostSettingsModel Create(HostSettingsData Data) => new()
        {
            DeveloperMode = Data.DeveloperMode,
            Colorscheme = Data.Colorscheme
        };

        public HostSettingsData Apply(HostSettingsData data) => data with
        {
            DeveloperMode = DeveloperMode,
            Colorscheme = Colorscheme
        };

        [Display(Name = "Developer Mode", Description = "Enable developer mode. You may need to manually refresh the page after changing this setting.")]
        [ModelBinder(typeof(CheckboxModelBinder))]
        public required bool DeveloperMode { get; set; }

        [Display(Name = "Colorscheme", Description = "Colorscheme for the Odyssey UI")]
        public required OdysseyColorscheme Colorscheme { get; set; }
    }
}
