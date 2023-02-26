using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    public class CreateEnemyViewSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Alive, UnitInfo, RaceInfo>, Exc<ControllerByPlayer, UnitViewRef>> _rawEnemies = default;
        private EcsCustomInject<PackedPrefabData> _prefabData = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index in _rawEnemies.Value)
            {
                ref UnitInfo unitInfo = ref _rawEnemies.Pools.Inc2.Get(index);
                ref RaceInfo raceInfo = ref _rawEnemies.Pools.Inc3.Get(index);

                UnitView enemyView = _prefabData.Value.EnemyUnits[raceInfo.Type][(int)unitInfo.Type - 1];
                GameObject enemyObject = Object.Instantiate(enemyView.gameObject, Vector3.up, Quaternion.identity);

                ref UnitViewRef enemyRef = ref world.GetPool<UnitViewRef>().Add(index);
                enemyRef.Value = enemyView;

                ref Moving moving = ref world.GetPool<Moving>().Add(index);
                moving = enemyView.Moving;

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
    }
}