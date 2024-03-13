using System;
using System.Collections.Generic;
using System.Linq;
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
      var allEnemyPoints = new List<int>();

      foreach (int index in _points.Value)
      {
        UnitType unitType = _world.Get<SpawnPoint>(index).UnitType;
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
        UnitType unitType = _world.Get<UnitInfo>(index).Type;
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
      ref SpawnPoint unitPoint = ref _world.Get<SpawnPoint>(spawnEntity);
      return unitPoint.UnitSpawn;
    }

    private bool HasSpawnPoints() => _points.Value.GetEntitiesCount() > 0;

    private void PutInSpawn(int unit, Transform spawn)
    {
      UnitView unitView = _world.Get<UnitViewRef>(unit).Value;
      if (Utils.IsUnitOf(_world, unit, UnitType.Player))
      {
        Utils.SetTransform(unitView.transform, spawn);
      }
      else
      {
        NavMeshAgent agent = _world.Get<ControllerByAI>(unit).Agent;
        agent.Warp(spawn.position);
        agent.transform.rotation = spawn.rotation;
      }

      unitView.Rb.velocity = Vector3.zero;
    }

    private List<int> SelectEnemyPoints(List<int> allSpawnPoints)
    {
      int numberOfEnemies = GetEnemyCount();
      if (numberOfEnemies == 0)
        return null;

      if (allSpawnPoints.Count < numberOfEnemies)
        throw new ArgumentException("Spawn points for enemies is less than enemies themselves");

      var enemyPoints = new List<int>();
      for (int i = numberOfEnemies; i > 0; i--)
      {
        int index = Random.Range(0, allSpawnPoints.Count);
        enemyPoints.Add(allSpawnPoints[index]);

        allSpawnPoints.RemoveAt(index);
      }

      return enemyPoints;
    }

    private int GetEnemyCount()
    {
      var numberOfEnemies = 0;
      foreach (int index in _units.Value)
      {
        if (Utils.IsEnemy(_world, index))
          numberOfEnemies++;
      }

      return numberOfEnemies;
    }
  }
}