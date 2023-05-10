using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class GetDeveloperSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<GetDeveloperMessage>> _devMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<SceneInfo> _sceneInfo = default;

    public void Run(IEcsSystems systems)
    {
      EcsWorld world = systems.GetWorld();
      foreach (int _ in _devMsgs.Value)
        _sceneInfo.Value.DevTools = !_sceneInfo.Value.DevTools;

      if (!_sceneInfo.Value.DevTools) return;
      if (!world.Has<Opener>(Utils.GetPlayerEntity(world)))
        world.Add<Opener>(Utils.GetPlayerEntity(world));
    }
  }
}