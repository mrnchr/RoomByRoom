using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    internal class DieSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Health>, Exc<DieCommand>> units = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index in units.Value)
            {
                if (world.GetComponent<Health>(index).Point == 0)
                {
                    world.AddComponent<DieCommand>(index);
                    UnityEngine.Debug.Log($"Entity {index} died");
                }
            }
        }
    }
}