using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class DamageSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<GetDamageCommand>> _commands = default;
        private EcsFilterInject<Inc<ArmorInfo, Equipped, ItemViewRef>> _armors = default;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            foreach(var index in _commands.Value)
            {
                float protection = GetTotalProtection(index);
                ref Health health = ref _world.GetPool<Health>().Get(index);
                ref GetDamageCommand damageCmd = ref _commands.Pools.Inc1.Get(index);
                ref PhysicalDamage physDamage = ref _world.GetPool<PhysicalDamage>().Get(damageCmd.Weapon);

                UnityEngine.Debug.Log($"Damage: {physDamage.Point}, protection: {protection}");
                if(physDamage.Point > protection)
                {
                    health.Point -= physDamage.Point - protection;
                    UnityEngine.Debug.Log($"Health: {health.Point}");
                }
                
                _commands.Pools.Inc1.Del(index);
            }
        }

        public float GetTotalProtection(int unit)
        {
            float total = 0;

            foreach(var index in _armors.Value)
            {
                ref Owned owned = ref _world.GetPool<Owned>().Get(index);
                if(owned.Owner == unit)
                {
                    ref Protection protection = ref _world.GetPool<Protection>().Get(index);
                    total += protection.Point;
                }
            }

            return total;
        }
    }
}