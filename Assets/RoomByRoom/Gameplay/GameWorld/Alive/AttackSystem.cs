using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class AttackSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<AttackCommand, Attackable, UnitViewRef>> _attacks = default;

        public void Run(IEcsSystems systems)
        {
            foreach(var attack in _attacks.Value)
            {
                ref Attackable attackable = ref _attacks.Pools.Inc2.Get(attack);
                ref UnitViewRef unitViewRef = ref _attacks.Pools.Inc3.Get(attack);
                unitViewRef.Value.PlayAttackAnimation(attackable.Weapon.Type);
            }
        }
    }
}