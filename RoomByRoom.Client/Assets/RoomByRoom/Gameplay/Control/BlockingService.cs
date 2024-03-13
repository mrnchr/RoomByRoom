using RoomByRoom.UI.Game;

namespace RoomByRoom
{
  public class BlockingService
  {
    private readonly GlobalState _globalState;

    public BlockingService(GlobalState globalState)
    {
      _globalState = globalState;
    }

    public WindowType CurrentState => _globalState.OpenedWindow;

    public bool IsBlocking() => CurrentState != WindowType.HUD;
    public bool IsPause() => _globalState.OpenedWindow == WindowType.Pause;
    public bool IsInventory() => _globalState.OpenedWindow == WindowType.Inventory;
  }
}