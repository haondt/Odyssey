using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Odyssey.Client.Authentication.Services
{
    public class ClientStartupService(IServiceProvider serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var participants = scope.ServiceProvider.GetServices<IClientStartupParticipant>();
            foreach (var participant in participants.OrderBy(q => q.Priority))
                await participant.OnStartupAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
