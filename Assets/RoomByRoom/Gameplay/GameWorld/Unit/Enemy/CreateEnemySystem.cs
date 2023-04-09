using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class CreateEnemySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsgs = Idents.Worlds.MessageWorld;
        private readonly EcsCustomInject<GameInfo> _gameInfo = default;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

            foreach(int index in _nextRoomMsgs.Value)
            {
                ref NextRoomMessage nextRoomMsg = ref message.GetComponent<NextRoomMessage>(index);
                if (nextRoomMsg.Room.Type == RoomType.Enemy)
                    CreateEnemies();
                else if(nextRoomMsg.Room.Type == RoomType.Boss)
                    CreateEnemy(UnitType.Boss, nextRoomMsg.Race.Type);
            }
        }

        private void CreateEnemies()
        {
            int numberOfEnemies = 0;
            int enemySize = 0;
            int[] enemyWeights = { 2, 3, 4, 5 };

            while (numberOfEnemies < 11 && enemySize < 21)
            {
                UnitType enemyType = UnitType.Humanoid; // FastRandom.EnemyType;
                CreateEnemy(enemyType, FastRandom.EnemyRace);

                numberOfEnemies++;
                enemySize += enemyWeights[(int)enemyType - 1];
            }
        }

        private void CreateEnemy(UnitType type, RaceType race)
        {
            int enemy = _world.NewEntity();

            _world.AddComponent<UnitInfo>(enemy)
                .Assign(x => { x.Type = type; return x; });

            _world.AddComponent<RaceInfo>(enemy)
                .Assign(x => { x.Type = race; return x; });

            _world.AddComponent<ControllerByAI>(enemy);

            if(type == UnitType.Humanoid)
                _world.AddComponent<Bare>(enemy);

            _world.AddComponent<Health>(enemy)
                .Assign(x =>
                    {
                        x.Point = FastRandom.GetUnitHP(_gameInfo.Value.RoomCount, type);
                        return x;
                    });
        }
    }
}