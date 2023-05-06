using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class SelectBonusSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<Bonus, BonusViewRef>> _bonus = default;
    private readonly EcsCustomInject<PlayerData> _playerData = default;
    private readonly EcsFilterInject<Inc<Selected>> _selected = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      int oldBonus = -1;
      foreach (int index in _selected.Value)
        oldBonus = index;

      int player = _world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
      UnitView playerView = _world.Get<UnitViewRef>(player).Value;
      var minDist = float.MaxValue;
      int newBonus = -1;

      foreach (int index in _bonus.Value)
      {
        BonusView bonusView = _world.Get<BonusViewRef>(index).Value;

        float sqrDist = (bonusView.transform.position - playerView.transform.position).sqrMagnitude;
        if (sqrDist < minDist)
        {
          minDist = sqrDist;
          newBonus = index;
        }
      }

      if (!IsBonus(newBonus))
        return;

      minDist = Mathf.Sqrt(minDist);
      if (!IsBonus(oldBonus))
      {
        if (!IsFar(minDist))
          Select(newBonus);
      }
      else
      {
        if (IsNew(newBonus, oldBonus))
        {
          Deselect(oldBonus);

          if (!IsFar(minDist))
            Select(newBonus);
        }
        else
        {
          if (IsFar(minDist))
            Deselect(oldBonus);
        }
      }
    }

    private static bool IsNew(int newBonus, int oldBonus) => newBonus != oldBonus;

    private bool IsFar(float distance) => distance > _playerData.Value.TakeItemDistance;

    private static bool IsBonus(int entity) => entity != -1;

    private void Select(int entity)
    {
      _world.Add<SelectCommand>(entity);
      _world.Add<Selected>(entity);
    }

    private void Deselect(int entity)
    {
      _world.Del<Selected>(entity);
      _world.Add<DeselectCommand>(entity);
    }
  }
}