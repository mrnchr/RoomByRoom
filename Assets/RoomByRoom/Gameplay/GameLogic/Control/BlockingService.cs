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

		public bool IsBlocking() => _gameInfo.OpenedWindow != WindowType.HUD;
		public bool IsPause() => _gameInfo.OpenedWindow == WindowType.Pause;
		public bool IsInventory() => _gameInfo.OpenedWindow == WindowType.Inventory;
	}
}