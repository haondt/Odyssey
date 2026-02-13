using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Models;
using Odyssey.Core.Constants;
using Odyssey.Persistence.Models;

namespace Odyssey.Client.Core.Services;

public class AuthenticationDataSeeder(
    UserManager<UserDataSurrogate> userManager,
    RoleManager<IdentityRole> roleManager,
    IUserSessionService userService,
    IOptions<AdminSettings> adminSettings) : IClientStartupParticipant
{
    public int Priority => 10000;


    public async Task OnStartupAsync()
    {
        foreach (var role in AuthConstants.Roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;
            var result = await roleManager.CreateAsync(new(role));
            if (!result.Succeeded)
                throw new InvalidOperationException($"Unable to register role \"{role}\" due to error(s): {string.Join('\n', result.Errors)}");
        }

        if (adminSettings.Value.RegisterDefaultAdminUser)
        {
            if (await userManager.FindByNameAsync(adminSettings.Value.DefaultAdminUsername!) is null)
            {
                var result = await userService.RegisterAdministratorAsync(adminSettings.Value.DefaultAdminUsername!, adminSettings.Value.DefaultAdminPassword!);
                if (!result.IsSuccessful)
                    throw new InvalidOperationException($"Unable to register default admin due to error(s): {string.Join('\n', result.Reason.Select(r => r.ToString()))}");
            }
        }
    }
}
