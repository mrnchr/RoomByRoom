using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Config.Data;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class SpawnBonusSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<SpawnCommand>, Exc<BonusViewRef>> _bonuses = default;
    private readonly EcsCustomInject<PrefabService> _prefabData = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _bonuses.Value)
      {
        PutItemInBonus(SpawnItem(index), SpawnBonus(index));
        _world.Del<SpawnCommand>(index);
      }
    }

    private void PutItemInBonus(ItemView itemView, BonusView bonusView)
    {
      Transform transform = itemView.transform;
      Transform place = bonusView.ItemPlace;

      transform.SetParent(place);
      transform.localPosition = -itemView.Center.localPosition;
      transform.rotation = place.rotation;
    }

    private BonusView SpawnBonus(int bonus)
    {
      BonusView bonusView = Object.Instantiate(_prefabData.Value.Prefabs.Bonus);
      bonusView.transform.position = _world.Get<SpawnCommand>(bonus).Coords;
      bonusView.Entity = bonus;
      _world.Add<BonusViewRef>(bonus)
        .Value = bonusView;
      return bonusView;
    }

    private ItemView SpawnItem(int bonus)
    {
      int item = _world.Get<Bonus>(bonus).Item;
      ItemView itemView = Object.Instantiate(GetItemPrefab(item));
      itemView.Entity = item;
      _world.Add<ItemViewRef>(item)
        .Value = itemView;
      return itemView;
    }

    // TODO: add artifact type
    private ItemView GetItemPrefab(int item) =>
      _prefabData.Value.GetItem(
        _world.Get<ItemInfo>(item).Type,
        Utils.GetEquipmentType(_world, item),
        _world.Get<Shape>(item).PrefabIndex);
  }
}