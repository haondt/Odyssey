using Haondt.Web.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Odyssey.Client.Display.Services;
using Odyssey.Core.Exceptions;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions.Reasons;
using Odyssey.UI.Core.Attributes;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Display.Components;
using Odyssey.UI.Display.Filters;

namespace Odyssey.UI.Display.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route(OdysseyRoutes.Display.Index)]
    [DisplaySession(Order = 40)]
    [AllowAnonymous]
    [EnforceBottomSheetContent(Uri = OdysseyRoutes.Display.Index, Type = typeof(DisplayBottomSheetContent))]
    public class DisplayController(IComponentFactory componentFactory,
        IDisplaySessionService displaySessionService,
        IClientDisplayService display,
        ICrockfordService crockford) : UIController
    {
        [HttpGet]
        public IResult Get() => TypedResults.Redirect(
            QueryHelpers.AddQueryString(
                OdysseyRoutes.Display.Party.Index, DisplaySessionAttribute.DisplayIdQueryParameter, displaySessionService.DisplayId.ToString()));

        [HttpGet(OdysseyRoutes.Display.Party.Index)]
        public async Task<IResult> GetParty()
        {
            if (await display.GetPartyAsync() is { HasValue: true, Value: var party })
                return await componentFactory.RenderComponentAsync<DisplayParty>();
            return await componentFactory.RenderComponentAsync<DisplayJoinParty>();
        }

        [HttpPost(OdysseyRoutes.Display.Party.Join.Index)]
        [ValidationState(typeof(FieldInvalidator))]
        public async Task<IResult> JoinParty([FromForm] DisplayJoinPartyModel joinData)
        {
            if (!string.IsNullOrEmpty(joinData.JoinCode))
                joinData.JoinCode = crockford.Normalize(joinData.JoinCode);

            await display.ConfigureDisplayProfileAsync(new()
            {
                Name = joinData.DisplayName,
                Type = OdysseyClientTypes.Browser,
            });
            var joinResult = await display.JoinPartyAsync(joinData.JoinCode);
            if (!joinResult.IsSuccessful)
            {
                return joinResult.Reason switch
                {
                    JoinPartyReason.PartyDoesNotExist => await RenderValidationComponentAsync(new()
                    {
                        [nameof(DisplayJoinPartyModel.JoinCode)] = "Party not found"
                    }),
                    _ => throw ExceptionFactory.CasesExhaustedException(joinResult.Reason),
                };
            }

            return await componentFactory.RenderComponentAsync<DisplayParty>();
        }

        [HttpPost(OdysseyRoutes.Display.Party.Leave.Index)]
        public async Task<IResult> LeaveParty([FromForm] string joinCode)
        {
            var result = await display.LeavePartyAsync(joinCode);
            if (result.TryGetReason(out var reason))
            {
                switch (reason)
                {
                    case LeavePartyReason.PartyDoesNotExist:
                        ResponseData.HxRedirect(OdysseyRoutes.Display.Party.Index);
                        return TypedResults.BadRequest();
                    default:
                        throw new ToastException("Unable to leave party")
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                }
            }

            return await componentFactory.RenderComponentAsync<DisplayJoinParty>();
        }
    }
}
