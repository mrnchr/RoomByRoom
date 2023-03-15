using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class MoveUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MoveCommand, UnitViewRef, Moving>> _units = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index in _units.Value)
            {
                Move(world, index);
            }
        }

        private void Move(EcsWorld world, int entity)
        {
            Vector3Int moveDirection = _units.Pools.Inc1.Get(entity).MoveDirection;
            ref UnitViewRef unitRef = ref _units.Pools.Inc2.Get(entity);
            ref Moving moving = ref _units.Pools.Inc3.Get(entity);
            Vector3 endDirection;

            if(unitRef.Value is PlayerView playerView)
            {
                endDirection = playerView.CameraHolder.transform.TransformDirection(moveDirection);
                endDirection.y = 0;
            }
            else 
            {
                endDirection = moveDirection;
            }

            endDirection = endDirection.normalized * moving.Speed;
            endDirection.y = unitRef.Value.Rb.velocity.y;

            unitRef.Value.Rb.velocity = endDirection;
        }
    }
}