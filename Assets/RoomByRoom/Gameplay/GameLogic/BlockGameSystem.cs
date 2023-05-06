using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;
using static UnityEngine.CursorLockMode;

namespace RoomByRoom
{
  public class BlockGameSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<GameLoadedMessage>> _loadedMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsFilterInject<Inc<WindowChangedMessage>> _windowMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<BlockingService> _blockingSvc = default;

    public void Run(IEcsSystems systems)
    {
      if (_windowMsgs.Value.GetEntitiesCount() <= 0 && _loadedMsgs.Value.GetEntitiesCount() <= 0) return;
      Time.timeScale = _blockingSvc.Value.IsBlocking() ? 0 : 1;
      Cursor.lockState = _blockingSvc.Value.IsBlocking() ? None : Locked;
    }
  }
}