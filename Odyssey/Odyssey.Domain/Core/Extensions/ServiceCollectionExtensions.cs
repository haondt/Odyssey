using Haondt.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Domain.Authentication.Models;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;

namespace Odyssey.Domain.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RouteSettings>(configuration.GetSection(nameof(RouteSettings)));
            services.Configure<AuthenticationSettings>(configuration.GetSection(nameof(AuthenticationSettings)));
            services.AddScoped<IUserService, UserService>();
            services.Configure<AdminSettings>(configuration.GetSection(nameof(AdminSettings)));
            AdminSettings.Validate(services.AddOptions<AdminSettings>()).ValidateOnStart();
            services.AddScoped<IDbSeeder, DbSeeder>();

            return services;

        }

        public static Task SeedDbAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
            return seeder.SeedAsync();
        }

    }
}
