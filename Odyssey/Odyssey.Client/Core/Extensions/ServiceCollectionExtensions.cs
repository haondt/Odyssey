using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Authentication.Models;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Models;
using Odyssey.Client.Core.Services;
using Odyssey.Client.Device.Models;
using Odyssey.Client.Device.Services;
using Odyssey.Client.Display.Models;
using Odyssey.Client.Display.Services;
using Odyssey.Client.Host.Services;

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
            services.AddSingleton<IStandaloneModelBinder, StandaloneModelBinder>();
            services.AddSingleton(typeof(ISignalRConnectionRegistry<>), typeof(SignalRConnectionRegsitry<>));

            // orleans
            services.AddHostedService<ClientStartupService>();

            // authentication
            services.Configure<AuthenticationSettings>(configuration.GetSection(nameof(AuthenticationSettings)));
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<HostSessionContext>();
            services.AddScoped<IHostSessionService, HostSessionService>();
            services.AddScoped<IClientStartupParticipant, AuthenticationDataSeeder>();
            services.AddSingleton<ISignalRScopeFactory, SignalRScopeFactory>();

            // games
            services.AddSingleton<IClientGameRegistry, ClientGameRegistry>();

            // host
            services.AddScoped<IClientHostService, ClientHostService>();

            // display
            services.AddScoped<DisplaySessionContext>();
            services.AddScoped<IDisplaySessionService, DisplaySessionService>();
            services.AddScoped<IClientDisplayService, ClientDisplayService>();

            // device
            services.AddScoped<DeviceSessionContext>();
            services.AddScoped<IDeviceSessionService, DeviceSessionService>();
            services.AddScoped<IClientDeviceService, ClientDeviceService>();

            return services;
        }
    }
}
