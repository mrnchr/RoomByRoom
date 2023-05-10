using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Config.Data;
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
    private readonly EcsCustomInject<EnemyData> _enemyData = default;
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

        if (Utils.IsPlayer(_world, index))
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
      Utils.IsPlayer(_world, entity)
        ? _savedData.Value.Player.MovableCmp
        : _enemyData.Value.MovableCmp;

    private Jumpable GetJumping(int entity, GroundUnitView unit) =>
      Utils.IsPlayer(_world, entity)
        ? _savedData.Value.Player.JumpableCmp
        : _enemyData.Value.JumpableCmp;

    private UnitView InstantiateUnit(int entity) =>
      Object.Instantiate(GetUnitPrefab(entity));

    // TODO: load player depending on his race
    private UnitView GetUnitPrefab(int entity) =>
      SelectUnit(_world.Get<UnitInfo>(entity).Type, _world.Get<RaceInfo>(entity).Type);


    private UnitView SelectUnit(UnitType type, RaceType race) =>
      type switch
      {
        UnitType.Boss   => _prefabSvc.Value.Prefabs.BossUnits[(int)race - 1],
        UnitType.Player => _prefabSvc.Value.Prefabs.BasePlayerUnit,
        _               => _prefabSvc.Value.GetEnemies(race)[(int)type - 1]
      };
  }
}