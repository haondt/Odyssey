using System.ComponentModel.DataAnnotations;
using Haondt.Core.Models;

namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public record HostDeviceData
    {
        [Id(0)]
        public Optional<PartyMemberId> PlayerAssignmentDelegatedTo { get; set; }
    }
}
