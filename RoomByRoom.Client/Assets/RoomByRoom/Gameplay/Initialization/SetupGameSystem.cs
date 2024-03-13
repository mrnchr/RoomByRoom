using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.UI.Game;

namespace RoomByRoom
{
  public class SetupGameSystem : IEcsInitSystem
  {
    private readonly EcsCustomInject<GameMediator> _mediator = default;
    private readonly EcsCustomInject<ProgressData> _savedData = default;
    private readonly EcsCustomInject<GlobalState> _globalState = default;

    public void Init(IEcsSystems systems)
    {
      _mediator.Value.UpdateRoomCount(_savedData.Value.Game.RoomCount.ToString());

      _globalState.Value.OpenedWindow = WindowType.HUD;
      _mediator.Value.TurnWindow(WindowType.HUD);
    }
  }
}