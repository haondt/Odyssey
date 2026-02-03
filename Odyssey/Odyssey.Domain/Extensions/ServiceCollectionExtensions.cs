using Haondt.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Odyssey.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOdysseyDomainServices(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}
