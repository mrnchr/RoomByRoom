using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class CreateEnemySystem : IEcsRunSystem
    {
        EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;

        public void Run(IEcsSystems systems)
        {
            int numberOfEnemies = 0;
            int enemySize = 0;
            int[] enemySizes = { 2, 3, 4, 5 };

            EcsWorld world = systems.GetWorld();

            foreach(int room in _nextRoom.Value)
            {
                // Create enemies with random race and random type
                while(numberOfEnemies < 8 && enemySize < 26)
                {
                    int enemy = world.NewEntity();

                    // Add Alive component
                    // TODO: to change HP
                    ref Alive alive = ref world.GetPool<Alive>().Add(enemy);
                    alive.HP = 100;

                    // Add UnitInfo component
                    ref UnitInfo unit = ref world.GetPool<UnitInfo>().Add(enemy);
                    unit.Type = FastRandom.GetEnemyType();

                    // Add RaceInfo component
                    ref RaceInfo race = ref world.GetPool<RaceInfo>().Add(enemy);
                    race.Type = FastRandom.GetEnemyRace();

                    numberOfEnemies++;
                    enemySize += enemySizes[(int)unit.Type - 1];
                }
            }
            
        }
    }
}