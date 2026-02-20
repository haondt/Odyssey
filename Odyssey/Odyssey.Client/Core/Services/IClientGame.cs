using Microsoft.AspNetCore.Components;
using Odyssey.Domain.Core.Services;

namespace Odyssey.Client.Core.Services
{
    public interface IClientGame : IGame, IClientGameUIService
    {
    }

    public interface IClientGameUIService
    {
        Task<IComponent> GetEditBoardComponentAsync(Guid boardId);
    }
}
