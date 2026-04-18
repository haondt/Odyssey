using Microsoft.AspNetCore.Components;

namespace Odyssey.Domain.Core.Models
{
    public interface IGameParameters
    {
        int MaxPlayers { get; }
        int MinPlayers { get; }
        Func<IComponent> LobbyDescriptionComponentFactory { get; }
    }
}
