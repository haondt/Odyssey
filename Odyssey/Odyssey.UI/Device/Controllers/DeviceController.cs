using Haondt.Web.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Odyssey.Client.Device.Services;
using Odyssey.Core.Exceptions;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions.Reasons;
using Odyssey.UI.Core.Attributes;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Device.Components;
using Odyssey.UI.Device.Filters;

namespace Odyssey.UI.Device.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route(OdysseyRoutes.Device.Index)]
    [DeviceSession(Order = 40)]
    [AllowAnonymous]
    [EnforceBottomSheetContent(Uri = OdysseyRoutes.Device.Index, Type = typeof(DeviceBottomSheetContent))]
    public class DeviceController(IComponentFactory componentFactory,
        IDeviceSessionService deviceSessionService,
        IClientDeviceService device,
        ICrockfordService crockford) : UIController
    {
        [HttpGet]
        public IResult Get() => TypedResults.Redirect(
            QueryHelpers.AddQueryString(
                OdysseyRoutes.Device.Party.Index, DeviceSessionAttribute.DeviceIdQueryParameter, deviceSessionService.DeviceId.ToString()));

        [HttpGet(OdysseyRoutes.Device.Party.Index)]
        public async Task<IResult> GetParty()
        {
            if (await device.GetPartyAsync() is { HasValue: true })
                return await componentFactory.RenderComponentAsync<DeviceParty>();
            return await componentFactory.RenderComponentAsync<DeviceJoinParty>();
        }

        [HttpPost(OdysseyRoutes.Device.Party.Join.Index)]
        [ValidationState(typeof(FieldInvalidator))]
        public async Task<IResult> JoinParty([FromForm] DeviceJoinPartyModel joinData)
        {
            if (!string.IsNullOrEmpty(joinData.JoinCode))
                joinData.JoinCode = crockford.Normalize(joinData.JoinCode);

            await device.ConfigureDeviceProfileAsync(new()
            {
                Name = joinData.DeviceName,
                Type = OdysseyClientTypes.Browser,
            });
            var joinResult = await device.JoinPartyAsync(joinData.JoinCode);
            if (!joinResult.IsSuccessful)
            {
                return joinResult.Reason switch
                {
                    JoinPartyReason.PartyDoesNotExist => await RenderValidationComponentAsync(new()
                    {
                        [nameof(DeviceJoinPartyModel.JoinCode)] = "Party not found"
                    }),
                    _ => throw ExceptionFactory.CasesExhaustedException(joinResult.Reason),
                };
            }

            return await componentFactory.RenderComponentAsync<DeviceParty>();
        }

        [HttpPost(OdysseyRoutes.Device.Party.Leave.Index)]
        public async Task<IResult> LeaveParty([FromForm] string joinCode)
        {
            var result = await device.LeavePartyAsync(joinCode);
            if (result.TryGetReason(out var reason))
            {
                switch (reason)
                {
                    case LeavePartyReason.PartyDoesNotExist:
                        ResponseData.HxRedirect(OdysseyRoutes.Device.Party.Index);
                        return TypedResults.BadRequest();
                    default:
                        throw new ToastException("Unable to leave party")
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                }
            }

            return await componentFactory.RenderComponentAsync<DeviceJoinParty>();
        }
    }
}
