using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class CreateEnemyViewSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Health, UnitInfo, RaceInfo>, Exc<ControllerByPlayer, UnitViewRef>> _rawEnemies = default;
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

                // Add UnitViewRef component
                ref UnitViewRef enemyRef = ref world.GetPool<UnitViewRef>().Add(index);
                enemyRef.Value = enemyView;

                // Add Moving component
                ref Moving moving = ref world.GetPool<Moving>().Add(index);
                moving = enemyView.Moving;

                // Add Flying component for flying unit and
                // add Jumping component for ground one
                if(unitInfo.Type == UnitType.Flying)
                {
                    ref Flying flying = ref world.GetPool<Flying>().Add(index);
                }
                else 
                {
                    ref Jumping jumping = ref world.GetPool<Jumping>().Add(index);
                    jumping = ((GroundUnitView)enemyView).Jumping;
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