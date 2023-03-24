using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class PickMainPlayerWeaponSystem : IEcsInitSystem
    {
        private EcsFilterInject<Inc<WeaponInfo, Equipped>> _weapons = default;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            // Find and lace in hands player weapon which is not bow
            foreach(var index in _weapons.Value)
            {
                if (_weapons.Pools.Inc1.Get(index).Type != WeaponType.Bow)
                {
                    world.GetPool<InHands>().Add(index);
                    ref Owned owned = ref world.GetPool<Owned>().Get(index);
                    ref MainWeapon mainWeapon = ref world.GetPool<MainWeapon>().Add(owned.Owner);
                    mainWeapon.Entity = index;
                }
            }
        }
    }
}