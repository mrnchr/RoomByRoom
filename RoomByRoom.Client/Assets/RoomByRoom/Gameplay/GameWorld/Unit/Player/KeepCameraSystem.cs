using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class KeepCameraSystem : IEcsRunSystem
  {
    private readonly EcsCustomInject<BlockingService> _blockingSvc = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      if (_blockingSvc.Value.IsBlocking()) return;

      _world = systems.GetWorld();
      PlayerView player = GetPlayer();
      Vector3 cameraHolderPos = player.CameraHolder.position;

      player.Camera.position = Physics.Raycast(cameraHolderPos,
                                               GetDirectionToCamera(player), out RaycastHit hit, player.CameraDistance,
                                               player.WallMask)
        ? hit.point - GetDirectionToCamera(player) * 0.05f
        : cameraHolderPos + GetDirectionToCamera(player) * player.CameraDistance;
    }

    private Vector3 GetDirectionToCamera(PlayerView player) =>
      (player.Camera.position - player.CameraHolder.position).normalized;

    private PlayerView GetPlayer()
    {
      int entity = _world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
      return (PlayerView)_world.Get<UnitViewRef>(entity).Value;
    }
  }
}