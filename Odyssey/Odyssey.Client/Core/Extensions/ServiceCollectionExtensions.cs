using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Models;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Models;
using Odyssey.Client.Core.Services;
using Odyssey.Client.Games.Services;

namespace Odyssey.Client.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyClientServices(this IServiceCollection services, IConfiguration configuration)
        {

            // core
            services.Configure<RouteSettings>(configuration.GetSection(nameof(RouteSettings)));
            services.Configure<AdminSettings>(configuration.GetSection(nameof(AdminSettings)));
            AdminSettings.Validate(services.AddOptions<AdminSettings>()).ValidateOnStart();

            // orleans
            services.AddHostedService<ClientStartupService>();

            // authentication
            services.Configure<AuthenticationSettings>(configuration.GetSection(nameof(AuthenticationSettings)));
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IClientStartupParticipant, AuthenticationDataSeeder>();

            // games
            services.AddScoped<IGameRegistry, GameRegistry>();

            return services;

        }

    }
}
