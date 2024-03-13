using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Scene;
using RoomByRoom.Utility;
using UnityEngine.SceneManagement;

namespace RoomByRoom
{
  public class ReloadGameSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<PlayerDyingMessage>> _dieMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<ScenePreloader> _sceneSvc = default;
    private readonly EcsCustomInject<IGameSaveService> _savingSvc = default;

    public void Run(IEcsSystems systems)
    {
      foreach (int _ in _dieMsgs.Value)
      {
        _savingSvc.Value.SaveDefault();
        _sceneSvc.Value.PreloadScene(SceneManager.GetActiveScene().buildIndex);
      }
    }
  }
}