using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.Domain.Authentication.Models;
using Odyssey.Domain.Authentication.Services;
using Odyssey.UI.Authentication.Components;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Authentication.Controllers
{
    [Route("/auth")]
    public class AuthenticationController(
        ISessionService sessionService,
        IUserService userService) : UIController
    {
        [HttpGet("sign-in")]
        [AllowAnonymous]
        public Task<IResult> GetSignIn()
        {
            if (sessionService.IsAuthenticated)
                return Task.FromResult<IResult>(TypedResults.Redirect("/"));

            return ComponentFactory.RenderComponentAsync<Components.SignIn>();
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        [ValidationSummary(typeof(SignInPanel), Components.SignInPanel.Id)]
        public async Task<IResult> SignIn([FromForm] SignInModel signIn, [FromQuery] string? returnUrl)
        {
            var result = await userService.TrySignInAsync(signIn.Username, signIn.Password);
            if (!result.IsSuccessful)
                return await RenderValidationSummaryComponent(result.Reason);

            if (returnUrl != null)
                ResponseData.HxRedirect(returnUrl);
            else
                ResponseData.HxRedirect("/");
            return TypedResults.Ok();
        }

        [HttpPost("sign-out")]
        [AllowAnonymous]
        public async Task<IResult> AuthenticationSignOut()
        {
            await userService.SignOutAsync();
            ResponseData.HxRedirect("/");
            return TypedResults.Ok();
        }
    }
}
