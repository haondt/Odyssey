using Haondt.Core.Extensions;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.Client.Authentication.Services;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.UI.Authentication.Components;
using Odyssey.UI.Authentication.Models.Authentication;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Authentication.Controllers
{
    [Route(OdysseyRoutes.Auth.Index)]
    public class AuthenticationController(
        ISessionService sessionService,
        IUserSessionService userService,
        ICachedDataRepository<ServerSettings> serverSettings) : UIController
    {
        [HttpGet(OdysseyRoutes.Auth.SignIn)]
        [AllowAnonymous]
        public Task<IResult> GetSignIn()
        {
            if (sessionService.IsAuthenticated)
                return Task.FromResult<IResult>(TypedResults.Redirect(OdysseyRoutes.Home));

            return ComponentFactory.RenderComponentAsync<Components.SignIn>();
        }

        [HttpPost(OdysseyRoutes.Auth.SignIn)]
        [AllowAnonymous]
        [ValidationState(typeof(SignInPanel), Components.SignInPanel.Id)]
        public async Task<IResult> SignIn([FromForm] SignInModel signIn, [FromQuery] string? returnUrl)
        {
            var result = await userService.TrySignInAsync(signIn.Username, signIn.Password);
            if (!result.IsSuccessful)
                return await RenderValidationComponent(new() { [nameof(SignInModel)] = result.Reason });

            if (returnUrl != null)
                ResponseData.HxRedirect(returnUrl);
            else
                ResponseData.HxRedirect(OdysseyRoutes.Home);
            return TypedResults.Ok();
        }

        [HttpGet(OdysseyRoutes.Auth.Register)]
        [AllowAnonymous]
        public async Task<IResult> GetRegister()
        {
            if (sessionService.IsAuthenticated)
                return TypedResults.Redirect(OdysseyRoutes.Home);

            if (!(await serverSettings.GetDataAsync(ServerSettings.Key)).Data.OpenRegistration.Or(false))
                return TypedResults.Redirect(OdysseyRoutes.Home);

            return await ComponentFactory.RenderComponentAsync<Register>();
        }

        [HttpPost(OdysseyRoutes.Auth.Register)]
        [AllowAnonymous]
        [ValidationState(typeof(RegisterPanel), RegisterPanel.Id)]
        public async Task<IResult> Register([FromForm] RegisterModel register)
        {
            // TODO: waiting till we are ready for invite code generation
            throw new NotImplementedException();
        }

        [HttpPost(OdysseyRoutes.Auth.SignOut)]
        [AllowAnonymous]
        public async Task<IResult> AuthenticationSignOut()
        {
            await userService.SignOutAsync();
            ResponseData.HxRedirect(OdysseyRoutes.Index);
            return TypedResults.Ok();
        }
    }
}
