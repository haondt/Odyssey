using Odyssey.Domain.Core.Services;

namespace Odyssey.Domain.Core.Models
{
    public class GameHandle(IGame game)
    {
        public IGameSettingsService Settings => game;
        public IGameBoardsService Boards => game;
    }
}
