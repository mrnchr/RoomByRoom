using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class ReloadGameSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<PlayerDyingMessage>> _dieMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<ReloadSceneService> _sceneSvc = default;

    public void Run(IEcsSystems systems)
    {
      foreach (int _ in _dieMsgs.Value)
        _sceneSvc.Value.ReloadScene();
    }
  }
}