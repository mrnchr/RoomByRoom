using UnityEngine;
using UnityEngine.AI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
	public class EnemyAISystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<ControllerByAI, UnitViewRef>> _enemies = default;
		private readonly EcsCustomInject<EnemyConfig> _enemyCfg = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			Transform player = GetPlayerTransform();

			foreach (int index in _enemies.Value)
			{
				// TODO: to execute for all enemies
				if (!IsHumanoid(index))
					continue;

				Vector3 humanoidPos = GetHumanoidPosition(index);
				NavMeshAgent agent = _world.GetComponent<ControllerByAI>(index).Agent;
				agent.SetDestination(player.position);

				AddRotateComponent(index, ConvertToDirection(agent.steeringTarget - humanoidPos));

				agent.updatePosition = true;
				if (IsNearPlayer(humanoidPos, player.position))
				{
					if(CanAttack(index))
						_world.AddComponent<AttackCommand>(index);
					agent.updatePosition = false;
				}
			}
		}

		private bool CanAttack(int unit)
		{
			return !_world.HasComponent<CantAttack>(unit);
		}

		private void AddRotateComponent(int index, Vector3 rotateDirection)
		{
			_world.AddComponent<RotateCommand>(index)
				.Assign(x =>
				{
					x.RotateDirection = rotateDirection;
					return x;
				});
		}

		private Vector3 GetHumanoidPosition(int index) => GetHumanoidView(index).transform.position;

		private bool IsNearPlayer(Vector3 humanoidPosition, Vector3 playerPosition)
		{
			Vector3 distToPlayer = playerPosition - humanoidPosition;
			distToPlayer.y = 0;
			return distToPlayer.sqrMagnitude <= _enemyCfg.Value.AttackDistance * _enemyCfg.Value.AttackDistance;
		}

		private bool IsHumanoid(int index) => _world.GetComponent<UnitInfo>(index).Type == UnitType.Humanoid;

		private Transform GetPlayerTransform()
		{
			int playerEntity = _world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
			return _world.GetComponent<UnitViewRef>(playerEntity).Value.transform;
		}

		private HumanoidView GetHumanoidView(int entity) => (HumanoidView)_world.GetComponent<UnitViewRef>(entity).Value;

		private static Vector3 ConvertToDirection(Vector3 rawDirection)
		{
			rawDirection.y = 0;
			return rawDirection.normalized;
		}
	}
}