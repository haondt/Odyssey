using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odyssey.Core.Constants;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Admin.Controllers
{
    [Route(OdysseyRoutes.Roles.Admin.Index)]
    [Authorize(Roles = $"{AuthConstants.AdminRole}, {AuthConstants.SuperadminRole}")]
    public class AdminController : UIController
    {
    }
}
