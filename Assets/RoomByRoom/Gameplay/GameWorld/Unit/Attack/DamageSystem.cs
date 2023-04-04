using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

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
                ref Health health = ref _world.GetComponent<Health>(index);
                ref GetDamageCommand damageCmd = ref _commands.Pools.Inc1.Get(index);
                ref PhysicalDamage physDamage = ref _world.GetComponent<PhysicalDamage>(damageCmd.Entity);

                UnityEngine.Debug.Log($"Damage: {physDamage.Point}, protection: {protection}, health: {health.Point}");
                if (physDamage.Point > protection)
                {
                    health.Point -= physDamage.Point - protection;
                    if(health.Point < 0)
                        health.Point = 0;
                    UnityEngine.Debug.Log($"Health after damage: {health.Point}");
                }

                _commands.Pools.Inc1.Del(index);
            }
        }

        public float GetTotalProtection(int entity)
        {
            float total = 0;

            foreach(var index in _armors.Value)
            {
                if(_world.GetComponent<Owned>(index).Owner == entity)
                {
                    total += _world.GetComponent<Protection>(index).Point;
                }
            }

            return total;
        }
    }
}