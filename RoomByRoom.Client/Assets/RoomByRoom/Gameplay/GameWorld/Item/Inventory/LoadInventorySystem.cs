using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class LoadInventorySystem : IEcsInitSystem
  {
    private readonly EcsCustomInject<CharacteristicService> _charSvc = default;
    private readonly EcsCustomInject<ProgressData> _savedData = default;
    private readonly EcsCustomInject<KeepDirtyService> _keepDirtySvc = default;
    private readonly Dictionary<int, int> _boundItems = new Dictionary<int, int>();
    private readonly HashSet<int> _savedItems = new HashSet<int>();
    private InventorySave _inventorySave;
    private EcsWorld _world;

    public void Init(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      _inventorySave = _savedData.Value.InventorySave;

      CollectEntities();

      foreach (int item in _savedItems)
        _boundItems[item] = _world.NewEntity();

      LoadEntities();

      int player = Utils.GetPlayerEntity(_world);
      foreach (int itemEntity in _boundItems.Values)
      {
        _world.Add<Owned>(itemEntity)
          .Owner = player;

        AddItemToInventory(player, itemEntity);
      }

      _charSvc.Value.Calculate(player);
      _world.Get<UnitPhysicalProtection>(player)
        .Assign(x =>
        {
          x.CurrentPoint.Clamp(max: x.MaxPoint);
          return x;
        });

      _keepDirtySvc.Value.UpdateDirtyMessage(DirtyType.Slots);
    }

    private void AddItemToInventory(int player, int item)
    {
      Utils.AddItemToList(_world.Get<Inventory>(player).ItemList, _world.PackEntity(item));
      Utils.AddItemToList(
        _world.Has<Equipped>(item)
          ? _world.Get<Equipment>(player).ItemList
          : _world.Get<Backpack>(player).ItemList, _world.PackEntity(item));
    }

    private void CollectEntities()
    {
      ProcessComponents(_inventorySave.Item, CollectEntity);
      ProcessComponents(_inventorySave.Weapon, CollectEntity);
      ProcessComponents(_inventorySave.Armor, CollectEntity);
      ProcessComponents(_inventorySave.PhysDamage, CollectEntity);
      ProcessComponents(_inventorySave.PhysProtection, CollectEntity);
      ProcessComponents(_inventorySave.Equipped, CollectEntity);
      ProcessComponents(_inventorySave.Shape, CollectEntity);
      return;

      void CollectEntity<T>(BoundComponent<T> component)
        where T : struct =>
        _savedItems.Add(component.Entity);
    }

    private void LoadEntities()
    {
      ProcessComponents(_inventorySave.Item, LoadComponent);
      ProcessComponents(_inventorySave.Weapon, LoadComponent);
      ProcessComponents(_inventorySave.Armor, LoadComponent);
      ProcessComponents(_inventorySave.PhysDamage, LoadComponent);
      ProcessComponents(_inventorySave.PhysProtection, LoadComponent);
      ProcessComponents(_inventorySave.Equipped, LoadComponent);
      ProcessComponents(_inventorySave.Shape, LoadComponent);
      return;

      void LoadComponent<T>(BoundComponent<T> component)
        where T : struct =>
        _world.Add<T>(_boundItems[component.Entity])
          .Assign(_ => component.ComponentInfo);
    }

    private static void ProcessComponents<T>(List<BoundComponent<T>> components, Action<BoundComponent<T>> action)
      where T : struct =>
      components.ForEach(action);
  }
}