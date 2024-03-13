using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.UI.Game.Inventory;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
  public class DropItemSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<WindowChangedMessage>> _windowMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<BlockingService> _blockingSvc = default;
    private readonly EcsCustomInject<GameMediator> _mediator = default;
    private readonly EcsCustomInject<EquipService> _equipSvc = default;
    private EcsWorld _world;
    private EcsWorld _message;

    public void Run(IEcsSystems systems)
    {
      if (_blockingSvc.Value.IsBlocking() || _windowMsgs.Value.GetEntitiesCount() <= 0) return;
      _world = systems.GetWorld();
      _message = systems.GetWorld(Idents.Worlds.MessageWorld);
      foreach (EcsPackedEntity packed in _mediator.Value.ClearCan())
      {
        int item = _world.Unpack(packed);
        int player = Utils.GetPlayerEntity(_world);

        if (_world.Has<Equipped>(item))
          _equipSvc.Value.ChangeEquip(item);
        _world.Get<Backpack>(player).ItemList.Remove(packed);
        _world.Get<Inventory>(player).ItemList.Remove(packed);
        _world.Del<Owned>(item);

        _message.Add<InventoryChangedMessage>(_message.NewEntity());

        int bonus = _world.NewEntity();
        _world.Add<Bonus>(bonus)
          .Item = item;
        var playerView = (PlayerView)_world.Get<UnitViewRef>(player).Value;
        _world.Add<SpawnCommand>(bonus)
          .Coords = playerView.transform.position + playerView.Character.forward;
      }
    }
  }
}