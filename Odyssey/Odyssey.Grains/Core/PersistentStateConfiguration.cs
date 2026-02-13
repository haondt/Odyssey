namespace Odyssey.Grains.Core
{
    public class PersistentStateConfiguration : IPersistentStateConfiguration
    {
        public required string StateName { get; set; }

        public required string StorageName { get; set; }
    }
}
