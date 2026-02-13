namespace Odyssey.Client.Authentication.Services
{
    public interface IClientStartupParticipant
    {
        // lower = higher priority. lets set a base level of 10000
        int Priority { get; }
        Task OnStartupAsync();
    }
}
