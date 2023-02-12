using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class AfterJumpUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Jumping, Moving>> _units = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _units.Value)
            {
                ref Jumping jumping = ref _units.Pools.Inc1.Get(index);
                ref Moving moving = ref _units.Pools.Inc2.Get(index);

                if(!jumping.CanJump && moving.Rb.velocity.y > 1)
                    jumping.CanJump = true;

            }
        }
    }
}