using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace RoomByRoom
{
	public class EnemyAISystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<ControllerByAI, UnitViewRef>> _enemies = default;
		private readonly EcsCustomInject<EnemyData> _enemyData = default;
		private readonly EcsCustomInject<BlockingService> _blockingSvc = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			if (_blockingSvc.Value.IsBlocking()) return;
			
			_world = systems.GetWorld();
			Transform player = GetPlayerTransform();

			foreach (int index in _enemies.Value)
			{
				// TODO: to execute for all enemies
				if (!IsHumanoid(index))
					continue;

				Vector3 humanoidPos = GetHumanoidPosition(index);
				Vector3 moveDir;
				Vector3 rotateDir;
				NavMeshAgent agent = _world.Get<ControllerByAI>(index).Agent;
				if (!IsNearPlayer(humanoidPos, player.position))
				{
					agent.SetDestination(player.position);

					moveDir = (agent.nextPosition - humanoidPos).normalized;
					rotateDir = ConvertToDirection(agent.steeringTarget - humanoidPos);
				}
				else
				{
					agent.ResetPath();
					agent.Warp(humanoidPos);
					_world.Add<AttackCommand>(index);
					moveDir = Vector3.zero;
					rotateDir = ConvertToDirection(player.position - humanoidPos);
				}

				AddRotateComponent(index, rotateDir);

				_world.Add<MoveCommand>(index)
					.MoveDirection = moveDir;
			}
		}

		private void AddRotateComponent(int index, Vector3 rotateDirection) =>
			_world.Add<RotateCommand>(index)
				.Assign(x =>
				{
					x.RotateDirection = rotateDirection;
					return x;
				});

		private Vector3 GetHumanoidPosition(int index) => GetHumanoidView(index).transform.position;
		private HumanoidView GetHumanoidView(int entity) => (HumanoidView)_world.Get<UnitViewRef>(entity).Value;

		private bool IsNearPlayer(Vector3 enemyPos, Vector3 playerPos) =>
			(playerPos - enemyPos).sqrMagnitude <= Mathf.Pow(_enemyData.Value.AttackDistance, 2);

		private bool IsHumanoid(int index) => _world.Get<UnitInfo>(index).Type == UnitType.Humanoid;

		private Transform GetPlayerTransform()
		{
			int playerEntity = _world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
			return _world.Get<UnitViewRef>(playerEntity).Value.transform;
		}


		private static Vector3 ConvertToDirection(Vector3 rawDirection)
		{
			rawDirection.y = 0;
			return rawDirection.normalized;
		}
	}
}