using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.UI.Game.Inventory;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class UpdateDirtySystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<DirtyMessage>> _dirtyMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<GameMediator> _mediator = default;
    private readonly EcsCustomInject<ProgressData> _progress = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

      foreach (int index in _dirtyMsgs.Value)
      {
        DirtyType dirtyFlags = message.Get<DirtyMessage>(index).DirtyFlags;

        if ((dirtyFlags & DirtyType.Slots) != 0)
          UpdateInventory();
        if ((dirtyFlags & DirtyType.PlayerModel) != 0)
          _mediator.Value.UpdatePlayerRender(GetPlayer());
        if ((dirtyFlags & DirtyType.RoomCount) != 0)
          _mediator.Value.UpdateRoomCount(_progress.Value.Game.RoomCount.ToString());

        message.DelEntity(index);
      }
    }

    private PlayerView GetPlayer()
    {
      int player = Utils.GetPlayerEntity(_world);
      return (PlayerView)_world.Get<UnitViewRef>(player).Value;
    }

    private void UpdateInventory()
    {
      int player = Utils.GetPlayerEntity(_world);
      _mediator.Value.CleanAllSlots();
      _world.Get<Inventory>(player)
        .ItemList.ForEach(x => _mediator.Value.AddItemToInventory(GetItemInfoForSlot(_world.Unpack(x))));
    }

    private ItemInfoForSlot GetItemInfoForSlot(int item) =>
      new ItemInfoForSlot
      {
        ItemEntity = _world.PackEntity(item),
        Type = _world.Get<ItemInfo>(item).Type,
        EqType = Utils.GetEquipmentType(_world, item),
        IsEquipped = _world.Has<Equipped>(item),
        Shape = _world.Get<ShapeInfo>(item).PrefabIndex
      };
  }
}