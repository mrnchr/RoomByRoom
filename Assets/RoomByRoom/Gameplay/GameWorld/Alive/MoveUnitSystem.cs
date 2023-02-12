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
            foreach(var index in _units.Value)
            {
                Move(index);
            }
        }

        private void Move(int entity)
        {
            Vector3Int moveDirection = _units.Pools.Inc1.Get(entity).MoveDirection;
            ref Moving moving = ref _units.Pools.Inc2.Get(entity);

            // change velocity
            Vector3 endDirection = moveDirection;
            endDirection *= moving.Speed;
            endDirection.y = moving.Rb.velocity.y;
            moving.Rb.velocity = endDirection;
        }
    }
}