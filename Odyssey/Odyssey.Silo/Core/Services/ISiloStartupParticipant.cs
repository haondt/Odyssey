namespace Odyssey.Silo.Core.Services
{
    public interface ISiloStartupParticipant
    {
        // lower = higher priority. lets set a base level of 10000
        int Priority { get; }
        Task OnStartupAsync();
    }
}