using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class AttackSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<AttackCommand, UnitViewRef>> _attacks = default;
        private EcsFilterInject<Inc<InHands, ItemViewRef>> _weapons = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var attack in _attacks.Value)
            {
                ref UnitViewRef unitViewRef = ref _attacks.Pools.Inc2.Get(attack);

                foreach(var weapon in _weapons.Value)
                {
                    if(world.GetPool<Owned>().Get(weapon).Owner == attack)
                        unitViewRef.Value.PlayAttackAnimation(world.GetPool<WeaponInfo>().Get(weapon).Type);
                }
            }
        }
    }
}