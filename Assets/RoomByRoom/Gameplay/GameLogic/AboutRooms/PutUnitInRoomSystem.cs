using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace RoomByRoom
{
	internal class PutUnitInRoomSystem : IEcsRunSystem
	{
		private readonly EcsFilterInject<Inc<SpawnPoint>> _points = default;
		private readonly EcsFilterInject<Inc<UnitViewRef, UnitInfo>> _units = default;
		private EcsWorld _world;

		public void Run(IEcsSystems systems)
		{
			if (!HasSpawnPoints())
				return;

			_world = systems.GetWorld();

			var playerPoint = 0;
			var bossPoint = 0;
			List<int> allEnemyPoints = new List<int>();

			foreach (int index in _points.Value)
			{
				UnitType unitType = _world.GetComponent<SpawnPoint>(index).UnitType;
				switch (unitType)
				{
					case UnitType.Player:
						playerPoint = index;
						break;
					case UnitType.Boss:
						bossPoint = index;
						break;
					default:
						allEnemyPoints.Add(index);
						break;
				}
			}

			var enemyPoints = SelectEnemyPoints(allEnemyPoints);

			foreach (int index in _units.Value)
			{
				UnitType unitType = _world.GetComponent<UnitInfo>(index).Type;
				int spawnEntity = GetSpawnEntity(unitType);
				PutInSpawn(index, GetSpawn(spawnEntity));
			}

			foreach (int index in _points.Value)
				_world.DelEntity(index);

			int GetSpawnEntity(UnitType unitType)
			{
				int spawnEntity;
				switch (unitType)
				{
					case UnitType.Player:
						spawnEntity = playerPoint;
						break;
					case UnitType.Boss:
						spawnEntity = bossPoint;
						break;
					default:
						spawnEntity = enemyPoints[0];
						enemyPoints.RemoveAt(0);
						break;
				}

				return spawnEntity;
			}
		}

		private Transform GetSpawn(int spawnEntity)
		{
			ref SpawnPoint unitPoint = ref _world.GetComponent<SpawnPoint>(spawnEntity);
			return unitPoint.UnitSpawn;
		}

		private bool HasSpawnPoints() => _points.Value.GetEntitiesCount() > 0;

		private void PutInSpawn(int index, Transform spawn)
		{
			UnitView unitView = _world.GetComponent<UnitViewRef>(index).Value;
			if (_world.GetComponent<UnitInfo>(index).Type != UnitType.Player)
			{
				NavMeshAgent agent = _world.GetComponent<ControllerByAI>(index).Agent;
				agent.Warp(spawn.position);
				agent.transform.rotation = spawn.rotation;
			}
			else
			{
				Utils.SetTransform(unitView.transform, spawn);
			}

			unitView.Rb.velocity = Vector3.zero;
		}

		private List<int> SelectEnemyPoints(List<int> allSpawnPoints)
		{
			var numberOfEnemies = 0;

			foreach (int unit in _units.Value)
			{
				if (Utils.IsEnemy(_world.GetComponent<UnitInfo>(unit).Type))
					numberOfEnemies++;
			}

			if (numberOfEnemies == 0)
				return null;

			List<int> enemyPoints = new List<int>();

			if (allSpawnPoints.Count < numberOfEnemies)
				throw new ArgumentException("Spawn points for enemies is less than enemies themselves");

			int index;
			while (numberOfEnemies > 0)
			{
				index = Random.Range(0, allSpawnPoints.Count);
				enemyPoints.Add(allSpawnPoints[index]);

				allSpawnPoints.RemoveAt(index);
				numberOfEnemies--;
			}

			return enemyPoints;
		}
	}
}