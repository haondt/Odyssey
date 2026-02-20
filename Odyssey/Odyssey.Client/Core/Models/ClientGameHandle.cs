using Odyssey.Client.Core.Services;
using Odyssey.Domain.Core.Models;

namespace Odyssey.Client.Core.Models
{
    public class ClientGameHandle(IClientGame game) : GameHandle(game)
    {
        public IClientGameUIService UI => game;
    }
}
