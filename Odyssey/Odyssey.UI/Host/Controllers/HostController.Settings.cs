using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Haondt.Web.Components;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odyssey.Client.Core.Models;
using Odyssey.Core.Models;
using Odyssey.UI.Core.Attributes;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;
using Odyssey.UI.Host.Components.Boards;
using Odyssey.UI.Host.Components.Settings;
using Odyssey.UI.Host.Models;
using Orleans.Storage;

namespace Odyssey.UI.Host.Controllers
{
    public partial class HostController
    {
        [HttpGet(OdysseyRoutes.Host.Settings.Index)]
        public async Task<IResult> GetSettings()
        {
            var settings = await hostService.GetHostSettingsAsync();
            return await ComponentFactory.RenderComponentAsync(new HostSettings
            {
                Data = settings
            });
        }

        [HttpPost(OdysseyRoutes.Host.Settings.Index)]
        [ValidationState(typeof(FieldInvalidator))]
        public async Task<IResult> UpdateSettings([FromForm] HostSettingsModel settingsModel)
        {
            var settings = await hostService.GetHostSettingsAsync();
            settings = settingsModel.Apply(settings);
            try
            {
                await hostService.SetHostSettingsAsync(settings);
            }
            catch (InconsistentStateException ex)
            {
                logger.LogError(ex, $"Caught {nameof(InconsistentStateException)} while updating settings");
                throw new ToastException("Settings were updated from another device. Reload the page to get the latest version.", ex)
                {
                    Severity = ToastSeverity.Error,
                    StatusCode = 409
                };
            }

            var layout = new AppendComponentLayout
            {
                Components = [
                ]
            };
            return await ComponentFactory.RenderComponentAsync(new Toast
            {
                Severity = ToastSeverity.Success,
                Text = "Settings updated"
            });
        }

        [HttpPost(OdysseyRoutes.Host.Settings.Developer.SeedDb.Index)]
        public async Task<IResult> SeedDeveloperData()
        {
            var settings = await hostService.GetHostSettingsAsync();
            if (!settings.DeveloperMode)
                throw new ToastException("Developer mode is not enabled")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            await hostService.SeedDeveloperDataAsync();
            return await ComponentFactory.RenderComponentAsync(new Toast
            {
                Severity = ToastSeverity.Success,
                Text = "Data seeded"
            });
        }
    }
}
