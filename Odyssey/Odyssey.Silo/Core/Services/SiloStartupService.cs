using Microsoft.Extensions.Hosting;

namespace Odyssey.Silo.Core.Services
{
    public class SiloStartupService(IEnumerable<ISiloStartupParticipant> participants) : IHostedService
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            foreach (var participant in participants.OrderBy(q => q.Priority))
                await participant.OnStartupAsync();
        }

        public Task StartAsync(CancellationToken cancellationToken) => Execute(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

