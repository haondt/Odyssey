using Haondt.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Authentication.Controllers
{
    [Route("/auth")]
    public class AuthenticationController(IComponentFactory componentFactory) : UIController(componentFactory)
    {
        [HttpGet("sign-in")]
        [AllowAnonymous]
        public Task<IResult> Get() => _componentFactory.RenderComponentAsync<Components.SignIn>();
    }
}
