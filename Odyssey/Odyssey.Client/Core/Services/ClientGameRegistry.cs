using Odyssey.Client.Core.Models;
using Odyssey.Domain.Core.Services;

namespace Odyssey.Client.Core.Services
{
    public class ClientGameRegistry(IEnumerable<IClientGame> games) : AbstractGameRegistry<IClientGame, ClientGameHandle>(games), IClientGameRegistry
    {
        public override ClientGameHandle GetGame(string gameId) => new(_games[gameId]);
    }
}
