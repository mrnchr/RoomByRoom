using System;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class CreateEnemySystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;
        private EcsCustomInject<PackedGameData> _gameData = default;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            int numberOfEnemies = 0;
            int enemySize = 0;
            int[] enemySizes = { 2, 3, 4, 5 };

            _world = systems.GetWorld();

            foreach(int room in _nextRoom.Value)
            {
                ref NextRoomMessage nextRoom = ref _nextRoom.Pools.Inc1.Get(room);
                if (nextRoom.Type == RoomType.Enemy)
                {
                    // Create enemies with random race and random type
                    while (numberOfEnemies < 11 && enemySize < 21)
                    {
                        UnitType enemyType = FastRandom.GetEnemyType();
                        CreateEnemy(100, enemyType, FastRandom.GetEnemyRace());

                        numberOfEnemies++;
                        enemySize += enemySizes[(int)enemyType - 1];
                    }
                }
                else if(nextRoom.Type == RoomType.Boss)
                {
                    CreateEnemy(100, UnitType.Boss, nextRoom.Race.Type);
                }
            }
        }

        private void CreateEnemy(int hp, UnitType enemyType, RaceType enemyRace)
        {
            int enemy = _world.NewEntity();

            // Add UnitInfo component
            ref UnitInfo unit = ref _world.GetPool<UnitInfo>().Add(enemy);
            unit.Type = enemyType;

            // Add RaceInfo component
            ref RaceInfo race = ref _world.GetPool<RaceInfo>().Add(enemy);
            race.Type = enemyRace;

            UnitEntity unitEntity = _gameData.Value.GetUnitEntity(enemyType, enemyRace);

            // Add Health component
            ref Health health = ref _world.GetPool<Health>().Add(enemy);
            health.Point = unitEntity.Health.Point;
        }
    }
}