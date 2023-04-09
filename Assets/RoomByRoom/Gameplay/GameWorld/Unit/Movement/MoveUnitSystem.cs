using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class MoveUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MoveCommand, UnitViewRef, Movable>> _units = default;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            foreach(var index in _units.Value)
            {
                Move(index);
            }
        }

        private void Move(int entity)
        {
            UnitView unitView = _world.GetComponent<UnitViewRef>(entity).Value;
            Vector3 endDirection = GetRawDirection(entity, unitView);

            endDirection = endDirection.normalized * GetSpeed(entity);
            endDirection.y = unitView.Rb.velocity.y;
            unitView.Rb.velocity = endDirection;
        }

        private float GetSpeed(int entity)
        {
            return _world.GetComponent<Movable>(entity).Speed;
        }

        private Vector3 GetRawDirection(int entity, UnitView unitView)
        {
            Vector3 endDirection;
            Vector3 moveDirection = _world.GetComponent<MoveCommand>(entity).MoveDirection;
            if (unitView is PlayerView playerView)
            {
                endDirection = playerView.CameraHolder.TransformDirection(moveDirection);
                endDirection.y = 0;
            }
            else
            {
                endDirection = moveDirection;
            }

            return endDirection;
        }
    }
}