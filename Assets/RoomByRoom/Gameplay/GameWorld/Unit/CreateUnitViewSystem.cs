
using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class CreateUnitViewSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Health, UnitInfo, RaceInfo>, Exc<UnitViewRef>> _units = default;
        private EcsCustomInject<SavedData> _savedData = default;
        private EcsCustomInject<AttackService> _attackSvc = default;
        private EcsCustomInject<PackedPrefabData> _prefabData = default;
        private EcsWorld _world;
        private int _playerEntity;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerEntity = _world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];

            foreach (int index in _units.Value)
            {
                UnitView unitView = InstantiateUnit(index);
                unitView.Entity = index;
                unitView.AttackCtr.SetService(_attackSvc.Value);

                _world.AddComponent<UnitViewRef>(index)
                    .Initialize(x => { x.Value = unitView; return x; });

                _world.AddComponent<Moving>(index)
                    .Initialize(x => x = GetMoving(index, unitView));

                if (unitView is GroundUnitView groundUnit)
                {
                    _world.AddComponent<Jumping>(index)
                        .Initialize(x => x = GetJumping(index, groundUnit));
                }
                else if (unitView is FlyingUnitView)
                {
                    _world.AddComponent<Flying>(index);
                }
            }
        }

        private Moving GetMoving(int entity, UnitView unit)
        {
            return IsPlayer(entity)
                ? _savedData.Value.Player.MovingCmp
                : unit.MovingCmp;
        }

        private Jumping GetJumping(int entity, GroundUnitView unit)
        {
            return IsPlayer(entity)
                ? _savedData.Value.Player.JumpingCmp
                : unit.JumpingCmp;
        }

        private UnitView InstantiateUnit(int entity)
        {
            return GameObject.Instantiate(
                GetUnitPrefab(entity))
                .GetComponent<UnitView>();
        }

        private GameObject GetUnitPrefab(int entity)
        {
            ref UnitInfo unitInfo = ref _world.GetComponent<UnitInfo>(entity);
            ref RaceInfo raceInfo = ref _world.GetComponent<RaceInfo>(entity);

            return IsPlayer(entity)
                ? _prefabData.Value.Prefabs.BasePlayerUnit.gameObject
                : SelectEnemy(unitInfo.Type, raceInfo.Type);
        }

        private bool IsPlayer(int entity) => entity == _playerEntity;

        private GameObject SelectEnemy(UnitType type, RaceType race)
        {
            UnitView enemy =
                type == UnitType.Boss
                    ? _prefabData.Value.Prefabs.BossUnits[(int)race - 1]
                    : _prefabData.Value.GetEnemies(race)[(int)type - 1];

            return enemy.gameObject;
        }
    }
}