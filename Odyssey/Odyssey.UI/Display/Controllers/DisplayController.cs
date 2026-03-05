using Haondt.Web.Services;
using Haondt.Web.UI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Odyssey.Client.Display.Services;
using Odyssey.Core.Exceptions;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Display.Components;
using Odyssey.UI.Display.Filters;

namespace Odyssey.UI.Display.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route(OdysseyRoutes.Display.Index)]
    [DisplaySession(Order = 40)]
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
        [ValidationState(typeof(DisplayJoinPartyPanel), DisplayJoinPartyPanel.Id)]
        public async Task<IResult> JoinParty([FromForm] DisplayJoinPartyModel joinData)
        {
            if (!string.IsNullOrEmpty(joinData.JoinCode))
                joinData.JoinCode = crockford.Normalize(joinData.JoinCode);

            await display.ConfigureDisplayProfile(new()
            {
                Name = joinData.DisplayName,
                Type = OdysseyClientTypes.Browser,
                Id = displaySessionService.DisplayId
            });
            var joinResult = await display.JoinPartyAsync(joinData.JoinCode);
            if (!joinResult.IsSuccessful)
            {
                return joinResult.Reason switch
                {
                    GrainInterfaces.Sessions.Reasons.JoinPartyReason.PartyDoesNotExist => await RenderValidationComponent(new()
                    {
                        [nameof(DisplayJoinPartyModel.JoinCode)] = "Party not found"
                    }),
                    _ => throw ExceptionFactory.CasesExhaustedException(joinResult.Reason),
                };
            }

            return await componentFactory.RenderComponentAsync<DisplayParty>();
        }
    }
}
