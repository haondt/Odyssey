using Haondt.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Odyssey.Core.Models;
using Odyssey.Domain.Core.Services;

namespace Odyssey.Client.Core.Services
{
    public interface IClientGame : IGame, IClientGameUIService
    {
    }

    public interface IClientGameUIService
    {
        Task<IComponent> GetEditBoardComponentAsync(OwnedEntityGuid id);
        Task<IComponent> GetResetEditBoardComponentAsync(OwnedEntityGuid id);
        Task<Optional<IComponent>> HandleBoardStateUpdateAsync(OwnedEntityGuid id, HttpContext context);

        Task<(IComponent PlayersSummary, IComponent GameSummary)> GetSessionSummaryComponentAsync(OwnedEntityGuid id);
    }
}
