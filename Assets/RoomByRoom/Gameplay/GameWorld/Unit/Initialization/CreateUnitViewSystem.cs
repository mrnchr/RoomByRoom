using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace RoomByRoom
{
	public class CreateUnitViewSystem : IEcsRunSystem
	{
		private readonly EcsCustomInject<AttackService> _attackSvc = default;
		private readonly EcsCustomInject<PrefabService> _prefabSvc = default;
		private readonly EcsCustomInject<Saving> _savedData = default;
		private readonly EcsCustomInject<SceneInfo> _sceneInfo = default;
		private readonly EcsFilterInject<Inc<UnitInfo>, Exc<UnitViewRef>> _units = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			foreach (int index in _units.Value)
			{
				UnitView unitView = InstantiateUnit(index);
				unitView.Entity = index;
				unitView.AttackCtr.SetService(_attackSvc.Value);

				_world.Add<UnitViewRef>(index)
					.Value = unitView;

				_world.Add<Movable>(index)
					.Assign(_ => GetMoving(index, unitView));

				if (Utils.IsUnitOf(_world, index, UnitType.Player))
					SetPlayerUnit(index, unitView);
				else
					SetEnemyUnit(index, unitView);

				if (unitView is GroundUnitView groundUnit)
				{
					_world.Add<Jumpable>(index)
						.Assign(_ => GetJumping(index, groundUnit));

					if (groundUnit is HumanoidView)
						Utils.SetWeaponToAnimate(_world, index);
				}
				else if (unitView is FlyingUnitView)
				{
					_world.Add<Flyable>(index);
				}
			}
		}

		private void SetPlayerUnit(int index, UnitView unitView)
		{
			var playerView = (PlayerView)unitView;
			playerView.Camera = _sceneInfo.Value.MainCamera.transform;
			playerView.Camera.SetParent(playerView.CameraHolder);
			playerView.HealthBar = _sceneInfo.Value.HealthBar;
			playerView.ArmorBar = _sceneInfo.Value.ArmorBar;
		}

		private void SetEnemyUnit(int index, UnitView unitView)
		{
			SetNavMeshAgent(index, unitView);
		}

		private void SetNavMeshAgent(int index, UnitView unitView)
		{
			var agent = unitView.GetComponent<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updatePosition = false;
			agent.speed = GetMoving(index, unitView).Speed;

			_world.Get<ControllerByAI>(index)
					.Agent = agent;
		}

		private Movable GetMoving(int entity, UnitView unit) =>
			Utils.IsUnitOf(_world, entity, UnitType.Player)
				? _savedData.Value.Player.MovableCmp
				: unit.MovableCmp;

		private Jumpable GetJumping(int entity, GroundUnitView unit) =>
			Utils.IsUnitOf(_world, entity, UnitType.Player)
				? _savedData.Value.Player.JumpableCmp
				: unit.JumpableCmp;

		private UnitView InstantiateUnit(int entity) =>
			Object.Instantiate(GetUnitPrefab(entity));

		private UnitView GetUnitPrefab(int entity)
		{
			UnitType type = _world.Get<UnitInfo>(entity).Type;
			RaceType race = _world.Get<RaceInfo>(entity).Type;

			// TODO: load player depending on his race
			return Utils.IsUnitOf(_world, entity, UnitType.Player)
				? _prefabSvc.Value.Prefabs.BasePlayerUnit
				: SelectEnemy(type, race);
		}


		private UnitView SelectEnemy(UnitType type, RaceType race) =>
			type == UnitType.Boss
				? _prefabSvc.Value.Prefabs.BossUnits[(int)race - 1]
				: _prefabSvc.Value.GetEnemies(race)[(int)type - 1];
	}
}