using RoomByRoom.UI.Game;

namespace RoomByRoom
{
	public class BlockingService
	{
		private readonly GameInfo _gameInfo;
		
		public BlockingService(GameInfo gameInfo)
		{
			_gameInfo = gameInfo;
		}

		public WindowType CurrentState => _gameInfo.OpenedWindow;

		public bool IsBlocking() => CurrentState != WindowType.HUD;
		public bool IsPause() => _gameInfo.OpenedWindow == WindowType.Pause;
		public bool IsInventory() => _gameInfo.OpenedWindow == WindowType.Inventory;
	}
}