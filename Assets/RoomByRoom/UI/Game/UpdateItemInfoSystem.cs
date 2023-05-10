using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Config.Data;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;
using UnityEditor;
using UnityEngine;

namespace RoomByRoom
{
  public class UpdateItemInfoSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<UpdateItemInfoMessage>> _updateMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<GameMediator> _mediator = default;
    private readonly EcsCustomInject<PrefabService> _prefabSvc = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _updateMsgs.Value)
      {
        var packed = _updateMsgs.Pools.Inc1.Get(index).Item;
        if (packed == null)
        {
          _mediator.Value.UpdateItemRender();
          _mediator.Value.UpdateItemDescription();
        }
        else
        {
          int entity = _world.Unpack((EcsPackedEntity)packed);
          ItemType type = _world.Get<ItemInfo>(entity).Type;
          _mediator.Value.UpdateItemDescription(ConstructChars(type, entity));
          _mediator.Value.UpdateItemRender(GetPrefab(entity, type));
        }

        _updateMsgs.Pools.Inc1.Del(index);
      }
    }

    private Characteristic[] ConstructChars(ItemType type, int entity) =>
      type switch
      {
        ItemType.Weapon => new[]
        {
          new Characteristic()
          {
            CharType = typeof(ItemPhysicalDamage), Value = _world.Get<ItemPhysicalDamage>(entity).Point
          }
        },
        ItemType.Armor => new[]
        {
          new Characteristic()
          {
            CharType = typeof(ItemPhysicalProtection), Value = _world.Get<ItemPhysicalProtection>(entity).Point
          }
        },
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
      };

    private ItemView GetPrefab(int entity, ItemType itemType)
    {
      int prefabIndex = _world.Get<Shape>(entity).PrefabIndex;
      int typeNumber = itemType == ItemType.Weapon
        ? (int)_world.Get<WeaponInfo>(entity).Type
        : (int)_world.Get<ArmorInfo>(entity).Type;

      return _prefabSvc.Value.GetItem(itemType, typeNumber, prefabIndex);
    }
  }
}