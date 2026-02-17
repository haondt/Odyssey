namespace Odyssey.Domain.Core.Models
{
    [GenerateSerializer]
    public abstract class GameSettings
    {
        public abstract string DisplayName { get; set; }
    }
}
