using Haondt.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odyssey.Core.Constants;
using Odyssey.UI.Core.Controllers;

namespace Odyssey.UI.Admin.Controllers
{
    [Route("/roles/admin")]
    [Authorize(Roles = $"{AuthConstants.AdminRole}, {AuthConstants.SuperadminRole}")]
    public class AdminController(IComponentFactory componentFactory) : UIController(componentFactory)
    {
    }
}
