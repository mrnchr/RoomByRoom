using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    internal class LoadPlayerSystem : IEcsInitSystem
    {
        private EcsCustomInject<SavedData> _savedData = default;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            int playerEntity = world.NewEntity();

            // Add RaceInfo component
            ref RaceInfo info = ref world.GetPool<RaceInfo>().Add(playerEntity);
            info = _savedData.Value.PlayerRace;
            
            // Add Alive component
            ref Alive alive = ref world.GetPool<Alive>().Add(playerEntity);
            alive = _savedData.Value.PlayerHP;

            // Add UnitInfo component
            ref UnitInfo unit = ref world.GetPool<UnitInfo>().Add(playerEntity);
            unit.Type = UnitType.Player;

            // Add Opener component
            // TODO: remove after tests
            world.GetPool<Opener>().Add(playerEntity);

            // Add ControllerByPlayer component
            world.GetPool<ControllerByPlayer>().Add(playerEntity);
        }
    }
}