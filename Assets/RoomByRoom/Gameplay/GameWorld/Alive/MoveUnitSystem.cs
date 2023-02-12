using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class MoveUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MoveCommand>> _units = default;

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
            ref Moving moving = ref world.GetPool<Moving>().Get(entity);

            // change velocity
            Vector3 endDirection = moveDirection;
            endDirection *= moving.Speed;
            endDirection.y = moving.Rb.velocity.y;
            moving.Rb.velocity = endDirection;

            // change rotation to move direction
            // if (moveDirection != Vector3Int.zero)
            // {
            //     // if jumping, i.e. not flying, to change only two axis 
            //     moving.Rb.transform.forward = new Vector3
            //     (
            //         moveDirection.x, 
            //         world.GetPool<Jumping>().Has(entity) ? moveDirection.y : 0, 
            //         moveDirection.z
            //     )
            //     .normalized;
            // }
        }
    }
}