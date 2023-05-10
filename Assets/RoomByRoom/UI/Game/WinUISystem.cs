using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class WinUISystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<RoomCleanedMessage>> _cleanedMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsFilterInject<Inc<GameLoadedMessage>> _loadedMsgs = Idents.Worlds.MessageWorld;
    private readonly EcsCustomInject<GameMediator> _mediator = default;

    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      Activate();
      Start();
      Deactivate();
    }

    private void Start()
    {
      foreach (int _ in _loadedMsgs.Value)
        _mediator.Value.SetStartUI(true);
    }

    private void Deactivate()
    {
      foreach (int _ in _nextRoomMsgs.Value)
        _mediator.Value.SetWinUI(false);
    }

    private void Activate()
    {
      if (_world.Get<RoomInfo>(Utils.GetRoomEntity(_world)).Type == RoomType.Start) return;
      foreach (int _ in _cleanedMsgs.Value)
        _mediator.Value.SetWinUI(true);
    }
  }
}