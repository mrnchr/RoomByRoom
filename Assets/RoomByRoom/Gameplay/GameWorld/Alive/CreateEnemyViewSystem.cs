using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class CreateEnemyViewSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Health, UnitInfo, RaceInfo>, Exc<ControllerByPlayer, UnitViewRef>> _rawEnemies = default;
        private EcsCustomInject<PackedGameData> _gameData = default;
        private EcsCustomInject<AttackService> _attackSvc = default;
        private EcsCustomInject<PackedPrefabData> _prefabData = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index in _rawEnemies.Value)
            {
                ref UnitInfo unitInfo = ref _rawEnemies.Pools.Inc2.Get(index);
                ref RaceInfo raceInfo = ref _rawEnemies.Pools.Inc3.Get(index);

                // Spawn enemy object by his entity's race and type
                GameObject enemy = GameObject.Instantiate(SelectEnemy(unitInfo.Type, raceInfo.Type));
                UnitView enemyView = enemy.GetComponent<UnitView>();
                enemyView.Entity = index;
                enemyView.AttackCtr.SetService(_attackSvc.Value);

                // Add UnitViewRef component
                ref UnitViewRef enemyRef = ref world.GetPool<UnitViewRef>().Add(index);
                enemyRef.Value = enemyView;

                // Add Moving component
                ref Moving moving = ref world.GetPool<Moving>().Add(index);
                moving = _gameData.Value.GetUnitEntity(unitInfo.Type, raceInfo.Type).Moving;

                // Add Flying component for flying unit and
                // add Jumping component for ground one
                if(unitInfo.Type == UnitType.Flying)
                {
                    FlyingUnitEntity unit = _gameData.Value.GetUnitTypeEntity<FlyingUnitEntity>(unitInfo.Type, raceInfo.Type);

                    ref Flying flying = ref world.GetPool<Flying>().Add(index);
                    if(unit != null)
                    {
                        flying = unit.Flying;
                    }
                }
                else 
                {
                    GroundUnitEntity unit = _gameData.Value.GetUnitTypeEntity<GroundUnitEntity>(unitInfo.Type, raceInfo.Type);

                    ref Jumping jumping = ref world.GetPool<Jumping>().Add(index);
                    if(unit != null)
                    {
                        jumping = unit.Jumping;
                    }
                }
            }
        }

        private GameObject SelectEnemy(UnitType type, RaceType race)
        {
            UnitView enemy;

            if(type == UnitType.Boss)
            {
                enemy = _prefabData.Value.Prefabs.BossUnits[(int)race - 1];
            }
            else 
            {
                enemy = _prefabData.Value.EnemyUnits[race][(int)type - 1];
            }

            return enemy.gameObject;
        }
    }
}