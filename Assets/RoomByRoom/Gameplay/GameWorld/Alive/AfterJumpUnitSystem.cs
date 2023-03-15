using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class AfterJumpUnitSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Jumping, UnitViewRef, CantJump>> _units = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var index in _units.Value)
            {
                ref Jumping jumping = ref _units.Pools.Inc1.Get(index);
                ref UnitViewRef unitRef = ref _units.Pools.Inc2.Get(index);

                if(unitRef.Value.Rb.velocity.y > 1)
                    _units.Pools.Inc3.Del(index);
            }
        }
    }
}