using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class PickMainPlayerWeaponSystem : IEcsInitSystem
    {
        private EcsFilterInject<Inc<WeaponInfo, Equipped>> _weapons = default;

        public void Init(IEcsSystems systems)
        {
            foreach(var index in _weapons.Value)
            {
                if(_weapons.Pools.Inc1.Get(index).Type != WeaponType.Bow)
                    systems.GetWorld().GetPool<InHands>().Add(index);
            }
        }
    }
}