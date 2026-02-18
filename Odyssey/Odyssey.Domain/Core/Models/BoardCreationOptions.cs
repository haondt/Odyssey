namespace Odyssey.Domain.Core.Models
{
    public class BoardCreationOptions
    {
        public required string Name { get; set; }
        public required string GameId { get; set; }
        public required string OwnerId { get; set; }
    }
}
