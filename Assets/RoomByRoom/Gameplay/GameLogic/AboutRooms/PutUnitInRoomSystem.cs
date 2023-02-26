using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    internal class PutUnitInRoomSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<UnitViewRef, UnitInfo, Moving>> _units = default;
        private EcsFilterInject<Inc<SpawnPoint>> _points = default;

        public void Run(IEcsSystems systems)
        {
            if(_points.Value.GetEntitiesCount() == 0)
            {
                return;
            }

            EcsWorld world = systems.GetWorld();

            int playerPoint = 0;
            int bossPoint = 0;
            List<int> allEnemyPoints = new List<int>();
            int numberOfEnemies = 0;

            // Define player, boss and enemy spawn points
            foreach(var point in _points.Value)
            {
                ref SpawnPoint spawnPoint = ref _points.Pools.Inc1.Get(point);
                if(spawnPoint.UnitType == UnitType.Player)
                {
                    playerPoint = point;
                }
                else if(spawnPoint.UnitType == UnitType.Boss)
                {
                    bossPoint = point;
                }
                else 
                {
                    allEnemyPoints.Add(point);
                }
            }

            // Calculate the number of enemies
            foreach(var enemy in _units.Value)
            {
                ref UnitInfo unitInfo = ref _units.Pools.Inc2.Get(enemy);
                if(unitInfo.Type != UnitType.Player && unitInfo.Type != UnitType.Boss)
                {
                    numberOfEnemies++;
                }
            }

            // select random spawn points where enemies will be moved in
            List<int> enemyPoints = SelectEnemyPoints(allEnemyPoints, numberOfEnemies);

            // Move unit depending on his type
            foreach(var unit in _units.Value)
            {
                ref Moving moving = ref _units.Pools.Inc3.Get(unit);
                ref UnitInfo unitInfo = ref _units.Pools.Inc2.Get(unit);
                SpawnPoint unitPoint;
                if(unitInfo.Type == UnitType.Player)
                {
                    unitPoint = _points.Pools.Inc1.Get(playerPoint);
                }
                else if (unitInfo.Type == UnitType.Boss)
                {
                    unitPoint = _points.Pools.Inc1.Get(bossPoint);
                }
                else 
                {
                    unitPoint = _points.Pools.Inc1.Get(enemyPoints[0]);
                    enemyPoints.RemoveAt(0);
                }

                moving.Rb.transform.position = unitPoint.UnitSpawn.transform.position;

                // Due to spawn at the same point enemies run away
                moving.Rb.velocity = UnityEngine.Vector3.zero;
            }
        }

        private List<int> SelectEnemyPoints(List<int> allSpawnPoints, int numberOfEnemies) 
        {
            List<int> enemyPoints = new List<int>();
            int index;
            
            if(allSpawnPoints.Count < numberOfEnemies)
            {
                throw new System.ArgumentException("Spawn points for enemies is less than enemies themselves");
            }

            while(numberOfEnemies > 0)
            {
                index = UnityEngine.Random.Range(0, allSpawnPoints.Count);
                enemyPoints.Add(allSpawnPoints[index]);

                allSpawnPoints.RemoveAt(index);
                numberOfEnemies--;
            }

            return enemyPoints;
        }
    }
}