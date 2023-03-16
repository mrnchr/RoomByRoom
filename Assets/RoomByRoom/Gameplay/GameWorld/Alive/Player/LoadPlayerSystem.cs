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
            int player = world.NewEntity();
            PlayerEntity playerEntity = _savedData.Value.Player;

            // Add RaceInfo component
            ref RaceInfo info = ref world.GetPool<RaceInfo>().Add(player);
            info = playerEntity.Race;
            
            // Add Health component
            ref Health health = ref world.GetPool<Health>().Add(player);
            health = playerEntity.Health;

            // Add UnitInfo component
            ref UnitInfo unit = ref world.GetPool<UnitInfo>().Add(player);
            unit.Type = UnitType.Player;

            // Add Opener component
            // TODO: remove after tests
            world.GetPool<Opener>().Add(player);

            // //Add Attackable component
            // // TODO: redo after tests
            // ref Attackable attackable = ref world.GetPool<Attackable>().Add(player);
            // attackable.Weapon = _savedData.Value.PlayerWeapon;

            // Add ControllerByPlayer component
            world.GetPool<ControllerByPlayer>().Add(player);
        }
    }
}