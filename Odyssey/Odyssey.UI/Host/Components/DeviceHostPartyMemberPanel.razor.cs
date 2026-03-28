using System.ComponentModel.DataAnnotations;
using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Haondt.Web.Core.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.UI.Host.Components
{
    public class DeviceHostPartyMemberPanelModel
    {
        public static DeviceHostPartyMemberPanelModel Create(Guid id, HostDeviceData Data) => new()
        {
            Id = id,
            PlayerAssignmentDelegatedTo = Data.PlayerAssignmentDelegatedTo.Unwrap(),
        };

        public HostDeviceData Apply(HostDeviceData data) => data with
        {
            PlayerAssignmentDelegatedTo = PlayerAssignmentDelegatedTo.AsOptional()
        };

        [Required]
        public required Guid Id { get; set; }

        [Display(Prompt = "Delegate player assignment to")]
        public PartyMemberId? PlayerAssignmentDelegatedTo { get; set; }
    }
}
