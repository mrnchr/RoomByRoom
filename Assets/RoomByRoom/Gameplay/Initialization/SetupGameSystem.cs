using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;

namespace RoomByRoom
{
  public class SetupGameSystem : IEcsInitSystem
  {
    private readonly EcsCustomInject<GameMediator> _mediator = default;
    private readonly EcsCustomInject<Saving> _savedData = default;
    private readonly EcsCustomInject<GameInfo> _gameInfo = default;

    public void Init(IEcsSystems systems)
    {
      _gameInfo.Value.RoomCount = _savedData.Value.GameSave.RoomCount;
      _mediator.Value.UpdateRoomCount(_gameInfo.Value.RoomCount.ToString());

      _gameInfo.Value.OpenedWindow = WindowType.HUD;
      _mediator.Value.TurnWindow(WindowType.HUD);
    }
  }
}