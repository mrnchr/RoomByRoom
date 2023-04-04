using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class MoveUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MoveCommand, UnitViewRef, Moving>> _units = default;
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
            ref Moving moving = ref _units.Pools.Inc3.Get(entity);
            Vector3 endDirection = GetRawDirection(entity, unitView);

            endDirection = endDirection.normalized * moving.Speed;
            endDirection.y = unitView.Rb.velocity.y;

            unitView.Rb.velocity = endDirection;
        }

        private Vector3 GetRawDirection(int entity, UnitView unitView)
        {
            Vector3 endDirection;
            Vector3Int moveDirection = _world.GetComponent<MoveCommand>(entity).MoveDirection;
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