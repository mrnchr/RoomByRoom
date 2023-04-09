using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class DamageSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<GetDamageMessage>> _messages = Idents.Worlds.MessageWorld;
        private readonly EcsFilterInject<Inc<ArmorInfo, Equipped, ItemViewRef>> _armors = default;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            foreach(int index in _messages.Value)
            {
                ref GetDamageMessage message = ref _messages.Pools.Inc1.Get(index);
                float protection = GetTotalProtection(message.Damaged);
                ref Health health = ref _world.GetComponent<Health>(message.Damaged);
                ref PhysicalDamage physDamage = ref _world.GetComponent<PhysicalDamage>(message.Weapon);

                // UnityEngine.Debug.Log($"Damage: {physDamage.Point}, protection: {protection}, health: {health.Point}");
                if (physDamage.Point > protection)
                {
                    health.Point -= physDamage.Point - protection;
                    if(health.Point < 0)
                        health.Point = 0;
                    // UnityEngine.Debug.Log($"Health after damage: {health.Point}");
                }
                    
                // To be created from the service because not to be deleted by DelHere
                _messages.Pools.Inc1.Del(index);
            }
        }

        private float GetTotalProtection(int entity)
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