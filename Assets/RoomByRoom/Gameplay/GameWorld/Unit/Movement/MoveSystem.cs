using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class MoveSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<MoveCommand>> _units = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _units.Value)
        Move(index);
    }

    private void Move(int entity)
    {
      UnitView unitView = _world.Get<UnitViewRef>(entity).Value;
      Vector3 endVelocity = GetVelocity(entity, unitView);
      unitView.Rb.velocity = endVelocity;

      if (unitView is HumanoidView humanoid)
        humanoid.AnimateRun(GetRawDirection(entity) != Vector3.zero);
    }

    private float GetSpeed(int entity) => _world.Get<Movable>(entity).Speed;

    private Vector3 GetVelocity(int entity, UnitView unitView)
    {
      Vector3 endDirection = GetRawDirection(entity);
      if (unitView is PlayerView playerView)
      {
        endDirection = playerView.CameraHolder.TransformDirection(endDirection);
        endDirection.y = 0;
      }

      endDirection = endDirection.normalized * GetSpeed(entity);
      endDirection.y = unitView.Rb.velocity.y;
      return endDirection;
    }

    private Vector3 GetRawDirection(int entity) => _world.Get<MoveCommand>(entity).MoveDirection;
  }
}