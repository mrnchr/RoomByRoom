using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class MoveUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MoveCommand, Moving>> _units = default;

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
            ref Moving moving = ref _units.Pools.Inc2.Get(entity);

            // change velocity
            Vector3 endDirection = moveDirection;
            endDirection *= moving.Speed;
            endDirection.y = moving.Rb.velocity.y;

            // if the player change velocity relatively camera direction
            ref UnitViewRef player = ref world.GetPool<UnitViewRef>().Get(entity);
            if(player.Value is PlayerView playerView)
            {
                moving.Rb.velocity = playerView.transform.TransformDirection(endDirection);
            }
            else 
            {
                moving.Rb.velocity = endDirection;
            }

        }
    }
}