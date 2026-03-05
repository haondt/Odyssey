namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public class HostDisplayData
    {
        [Id(0)]
        public bool PlaySounds { get; set; }
        [Id(1)]
        public bool ReflectSoundBoard { get; set; }
    }
}
