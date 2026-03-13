using Odyssey.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.GrainInterfaces.Sessions.Services
{
    public interface ISessionGrainFactory<TBoard, TGameState> : IGrainFactory<OwnedEntityGuid, ISessionGrain<TBoard, TGameState>>
        where TBoard : IDataStorageData<TBoard>
        where TGameState : IDataStorageData<TGameState>
    {
    }
}
