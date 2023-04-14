using UnityEngine;
using UnityEngine.AI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class CreateUnitViewSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<UnitInfo>, Exc<UnitViewRef>> _units = default;
		private readonly EcsCustomInject<SavedData> _savedData = default;
		private readonly EcsCustomInject<AttackService> _attackSvc = default;
		private readonly EcsCustomInject<PackedPrefabData> _prefabData = default;
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
					.Assign(x =>
					{
						x.Value = unitView;
						return x;
					});

				_world.AddComponent<Movable>(index)
					.Assign(x => GetMoving(index, unitView));

				if (!IsPlayer(index))
					SetNavMeshAgent(index, unitView);

				if (unitView is GroundUnitView groundUnit)
				{
					_world.AddComponent<Jumpable>(index)
						.Assign(x => GetJumping(index, groundUnit));
				}
				else if (unitView is FlyingUnitView)
				{
					_world.AddComponent<Flyable>(index);
				}
			}
		}

		private void SetNavMeshAgent(int index, UnitView unitView)
		{
			NavMeshAgent agent = unitView.GetComponent<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updatePosition = false;
			agent.speed = GetMoving(index, unitView).Speed;

			_world.GetComponent<ControllerByAI>(index)
				.Assign(x =>
				{
					x.Agent = agent;
					return x;
				});
		}

		private Movable GetMoving(int entity, UnitView unit)
		{
			return IsPlayer(entity)
				? _savedData.Value.Player.MovableCmp
				: unit.MovableCmp;
		}

		private Jumpable GetJumping(int entity, GroundUnitView unit)
		{
			return IsPlayer(entity)
				? _savedData.Value.Player.JumpableCmp
				: unit.JumpableCmp;
		}

		private UnitView InstantiateUnit(int entity)
		{
			return Object.Instantiate(
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