namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public record HostSettings
    {
        [Id(0)]
        public bool DeveloperMode { get; set; } = false;

        [Id(1)]
        public OdysseyColorscheme Colorscheme { get; set; } = OdysseyColorscheme.Dark;
    }
}
