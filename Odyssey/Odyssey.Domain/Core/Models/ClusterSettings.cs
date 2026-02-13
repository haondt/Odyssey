namespace Odyssey.Domain.Core.Models
{
    public class ClusterSettings
    {
        public required string ClusterId { get; set; }
        public required string ServiceId { get; set; }
        public required bool UseLocalhostClustering { get; set; }
    }
}
