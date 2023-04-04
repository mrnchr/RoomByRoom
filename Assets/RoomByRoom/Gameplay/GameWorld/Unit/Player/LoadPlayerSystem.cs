using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    internal class LoadPlayerSystem : IEcsInitSystem
    {
        private EcsCustomInject<SavedData> _savedData = default;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            int player = world.NewEntity();
            PlayerEntity playerEntity = _savedData.Value.Player;

            world.AddComponent<RaceInfo>(player)
                .Initialize(x => x = playerEntity.Race);

            world.AddComponent<Health>(player)
                .Initialize(x => x = playerEntity.HealthCmp);

            world.AddComponent<UnitInfo>(player)
                .Initialize(x => { x.Type = UnitType.Player; return x; });

            // // TODO: remove after tests
            world.AddComponent<Opener>(player);
            
            world.AddComponent<ControllerByPlayer>(player);
        }
    }
}